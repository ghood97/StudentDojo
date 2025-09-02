using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Services;


namespace StudentDojo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetClassroomById(int id)
    //{
    //    ClassroomDto? classroom = await _studentService.GetClassroomByIdAsync(id);
    //    if (classroom == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(classroom);
    //}

    [HttpPost]
    public async Task<IActionResult> CreateStudentAsync([FromBody] StudentCreateDto createDto)
    {
        StudentDto createdStudent = await _studentService.CreateStudentAsync(createDto);
        return Ok(createdStudent);
    }
}
