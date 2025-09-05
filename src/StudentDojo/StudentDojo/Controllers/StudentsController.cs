using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Hubs;
using StudentDojo.Services;


namespace StudentDojo.Controllers;
[Route("api/classrooms/{classroomId:int}/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IHubContext<PointHub> _pointHub;

    public StudentsController(IStudentService studentService, IHubContext<PointHub> pointHub)
    {
        _studentService = studentService;
        _pointHub = pointHub;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudentAsync([FromBody] StudentCreateDto createDto)
    {
        StudentDto createdStudent = await _studentService.CreateStudentAsync(createDto);
        return Ok(createdStudent);
    }

    [HttpPatch("{studentId:int}/points")]
    public async Task<IActionResult> IncrementPointsAsync(
        [FromRoute] int classroomId,
        [FromRoute] int studentId,
        [FromBody] int pointsDelta)
    {
        AwardPointsResult result = await _studentService.IncrementPointsForStudentAsync(classroomId, studentId, pointsDelta);
        if (result.Success)
        {
            await _pointHub.Clients
                .Group($"Classroom-{classroomId}")
                .SendAsync("PointsUpdated", studentId, result.NewPoints);
            return Ok(result.NewPoints);
        }
        else if (result.Error is ServiceError.NotFound)
        {
            return NotFound(new { Message = "Student not found" });
        }
        else
        {
            throw new Exception("Failed to increment points");
        }
    }
}
