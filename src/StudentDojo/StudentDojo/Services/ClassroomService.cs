using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using StudentDojo.Core.Data.Entities;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Services;

public interface IClassroomService
{
    Task<IEnumerable<ClassroomDto>> GetClassroomsAsync(int teacherId);
    Task<ClassroomDto> CreateClassroomAsync(ClassroomCreateDto createDto);
}

public class ClassroomService : IClassroomService
{
    private readonly StudentDojoDbContext _db;
    private readonly ITeacherService _teacherService;
    private readonly ILogger<ClassroomService> _logger;

    public ClassroomService(StudentDojoDbContext db, ITeacherService teacherService, ILogger<ClassroomService> logger)
    {
        _db = db;
        _teacherService = teacherService;
        _logger = logger;
    }

    public async Task<IEnumerable<ClassroomDto>> GetClassroomsAsync(int teacherId)
    {
        try
        {
            return await _db.Classrooms
            .Where(c => c.Teachers.Any(t => t.TeacherId == teacherId))
            .Select(c => new ClassroomDto()
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

    public async Task<ClassroomDto> CreateClassroomAsync(ClassroomCreateDto createDto)
    {
        TeacherDto currentTeacher = await _teacherService.GetCurrentTeacherAsync();
        Classroom newClassroom = new()
        {
            ClassName = createDto.ClassName,
            Teachers = new List<TeacherClassroom>
            {
                new TeacherClassroom { TeacherId = currentTeacher.Id }
            }
        };

        _db.Classrooms.Add(newClassroom);
        await _db.SaveChangesAsync();
        return new ClassroomDto(newClassroom);
    }
}
