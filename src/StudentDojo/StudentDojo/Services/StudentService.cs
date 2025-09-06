using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using StudentDojo.Core.Data.Entities;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Services;

public interface IStudentService
{
    Task<StudentDto> CreateStudentAsync(StudentCreateDto createDto);
    Task<int> GetPointsAsync(int studentId);
    Task<AwardPointsResult> IncrementPointsForStudentAsync(int classroomId, int studentId, int pointsDelta);
    Task<RedeemPointsResult> RedeemPointsForStudentAsync(int classroomId, int studentId, int points);
}

public class StudentService : IStudentService
{
    private readonly StudentDojoDbContext _db;
    private readonly ILogger<StudentService> _logger;

    public StudentService(StudentDojoDbContext db, ILogger<StudentService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<StudentDto> CreateStudentAsync(StudentCreateDto createDto)
    {
        // TODO : Validate classroom exists and belongs to the current teacher
        Student newStudent = new()
        {
            Name = createDto.Name,
            Points = 0,
            ClassroomId = createDto.ClassroomId
        };

        _db.Students.Add(newStudent);
        await _db.SaveChangesAsync();
        return new StudentDto(newStudent);
    }

    public async Task<int> GetPointsAsync(int studentId)
    {
        Student? student = await _db.Students.FindAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for getting points", studentId);
            throw new InvalidOperationException("Student not found");
        }
        return student.Points;
    }

    public async Task<AwardPointsResult> IncrementPointsForStudentAsync(int classroomId, int studentId, int pointsDelta)
    {
        Student? student = await _db.Students.FindAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for incrementing points", studentId);
            return new(false, ServiceError.NotFound, 0);
        }
        if (student.ClassroomId != classroomId)
        {
            _logger.LogWarning("Student with ID {StudentId} does not belong to Classroom ID {ClassroomId}", studentId, classroomId);
            return new(false, ServiceError.Validation, student.Points);
        }
        student.Points += pointsDelta;
        await _db.SaveChangesAsync();
        return new(true, null, student.Points);
    }

    public async Task<RedeemPointsResult> RedeemPointsForStudentAsync(int classroomId, int studentId, int points)
    {
        Student? student = await _db.Students.FindAsync(studentId);
        if (student == null)
        {
            _logger.LogWarning("Student with ID {StudentId} not found for redeeming points", studentId);
            return new(false, RedeemPointsError.NotFound, 0);
        }
        if (student.ClassroomId != classroomId)
        {
            _logger.LogWarning("Student with ID {StudentId} does not belong to Classroom ID {ClassroomId}", studentId, classroomId);
            return new(false, RedeemPointsError.InvalidClassroom, student.Points);
        }
        if (student.Points < points)
        {
            _logger.LogWarning("Student with ID {StudentId} has insufficient points for redemption", studentId);
            return new(false, RedeemPointsError.InsufficientPoints, student.Points);
        }
        student.Points -= points;
        await _db.SaveChangesAsync();
        return new(true, null, student.Points);
    }
}

public sealed record AwardPointsResult(bool Success, ServiceError? Error, int NewPoints);

public sealed record RedeemPointsResult(bool Success, RedeemPointsError? Error, int NewPoints);

public enum RedeemPointsError
{
    NotFound,
    InsufficientPoints,
    InvalidClassroom
}