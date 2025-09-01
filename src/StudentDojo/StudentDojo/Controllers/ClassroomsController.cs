using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Services;


namespace StudentDojo.NewFolder;
[Route("api/[controller]")]
[ApiController]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetClassroomsByTeacher(int teacherId)
    {
        IEnumerable<ClassroomDto> classrooms = await _classroomService.GetClassroomsAsync(teacherId);
        return Ok(classrooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassroomAsync([FromBody] ClassroomCreateDto createDto)
    {
        try
        {
            ClassroomDto createdClassroom = await _classroomService.CreateClassroomAsync(createDto);
            return Ok(createdClassroom);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating classroom: {ex.Message}");
        }
    }
}
