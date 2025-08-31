using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Services;

public interface IClassroomService
{
    Task<IEnumerable<ClassroomDto>> GetClassroomsAsync(int teacherId);
    Task<ClassroomDto> CreateClassroom(string className);
}

public class ClassroomService : IClassroomService
{
    private readonly StudentDojoDbContext _db;
    private readonly ILogger<ClassroomService> _logger;

    public ClassroomService(StudentDojoDbContext db, ILogger<ClassroomService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<ClassroomDto>> GetClassroomsAsync(int teacherId)
    {
        try
        {
            return await _db.Classrooms
            .Where(c => c.Teachers.Any(t => t.TeacherId == teacherId))
            .Select(c => new ClassroomDto
            {
                Id = c.Id,
                ClassName = c.ClassName
            })
            .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching classrooms for teacherId {TeacherId}", teacherId);
            throw;
        }
    }

    public async Task<ClassroomDto> CreateClassroom(string className)
    {
        var classroom = new Core.Data.Entities.Classroom
        {
            ClassName = className
        };
        _db.Classrooms.Add(classroom);
        await _db.SaveChangesAsync();
        return new ClassroomDto
        {
            Id = classroom.Id,
            ClassName = classroom.ClassName
        };
    }
}
