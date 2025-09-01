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

    [HttpGet]
    public async Task<IActionResult> GetClassroomsByTeacher()
    {
        IEnumerable<ClassroomDto> classrooms = await _classroomService.GetClassroomsAsync();
        return Ok(classrooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClassroomAsync([FromBody] ClassroomCreateDto createDto)
    {
        ClassroomDto createdClassroom = await _classroomService.CreateClassroomAsync(createDto);
        return Ok(createdClassroom);
    }
}
