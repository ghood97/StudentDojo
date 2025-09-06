using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Hubs;
using StudentDojo.Services;


namespace StudentDojo.Controllers;
[Route("api/classrooms/{classroomId:int}/[controller]")]
[ApiController]
public class StudentsController : BaseController
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
            return NotFoundProblem("Student not found");
        }
        else if (result.Error is ServiceError.Validation)
        {
            return BadRequestProblem("Invalid Classroom", "Student does not belong to the specified classroom");
        }
        else
        {
            throw new Exception("Failed to increment points");
        }
    }

    [HttpPatch("{studentId:int}/redeem")]
    public async Task<IActionResult> RedeemPointsAsync(
        [FromRoute] int classroomId,
        [FromRoute] int studentId,
        [FromBody] int pointsToRedeem)
    {
        RedeemPointsResult result = await _studentService.RedeemPointsForStudentAsync(classroomId, studentId, pointsToRedeem);
        if (result.Success)
        {
            await _pointHub.Clients
                .Group($"Classroom-{classroomId}")
                .SendAsync("PointsUpdated", studentId, result.NewPoints);
            return Ok(result.NewPoints);
        }
        else if (result.Error is RedeemPointsError.NotFound)
        {
            return NotFoundProblem("Student not found");
        }
        else if (result.Error is RedeemPointsError.InvalidClassroom)
        {
            return BadRequestProblem("Invalid Classroom", "Student does not belong to the specified classroom");
        }
        else if (result.Error is RedeemPointsError.InsufficientPoints)
        {
            return BadRequestProblem("Insufficient points to redeem");
        }
        else
        {
            throw new Exception("Failed to redeem points");
        }
    }
}
