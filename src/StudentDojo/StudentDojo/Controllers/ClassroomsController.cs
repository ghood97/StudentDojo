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

    [HttpPost()]
    public async Task<IActionResult> CreateClassroom([FromBody] string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            return BadRequest("Class name cannot be empty.");
        }
        ClassroomDto newClassroom = await _classroomService.CreateClassroom(className);
        return CreatedAtAction(nameof(GetClassroomsByTeacher), new { teacherId = 0 }, newClassroom); // Adjust as needed
    }
}
