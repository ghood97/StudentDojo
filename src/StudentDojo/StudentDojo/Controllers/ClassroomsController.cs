using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Services;


namespace StudentDojo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClassroomsController : BaseController
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClassroomsByTeacher()
    {
        IEnumerable<ClassroomDto> classrooms = await _classroomService.GetClassroomsAsync();
        return Ok(classrooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassroomById(int id)
    {
        ClassroomDto? classroom = await _classroomService.GetClassroomByIdAsync(id);
        if (classroom == null)
        {
            return NotFoundProblem("Classroom not found");
        }
        return Ok(classroom);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassroomAsync([FromBody] ClassroomCreateDto createDto)
    {
        ClassroomDto createdClassroom = await _classroomService.CreateClassroomAsync(createDto);
        return Ok(createdClassroom);
    }
}
