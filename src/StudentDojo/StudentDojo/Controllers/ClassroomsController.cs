using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDojo.Client.Contracts.DataTransfer;
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
}
