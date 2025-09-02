using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Pages;
public partial class StudentCreate : ComponentBase
{
    private readonly ISnackbar _snackbar;
    private readonly IStudentApiService _studentService;
    private readonly INavService _nav;

    [Parameter] public int? ClassroomId { get; set; }
    private StudentCreateDto _createDto = new();

    public StudentCreate(ISnackbar snackbar, IStudentApiService studentService, INavService nav)
    {
        _snackbar = snackbar;
        _studentService = studentService;
        _nav = nav;
    }
    protected override void OnParametersSet()
    {
        if (ClassroomId.HasValue)
        {
            _createDto.ClassroomId = ClassroomId.Value;
        }
        base.OnParametersSet();
    }

    private async Task OnValidSubmitAsync(EditContext context)
    {
        ApiResponse<StudentDto> res = await _studentService.CreateStudentAsync(_createDto);
        if (res.IsSuccess)
        {
            _snackbar.Add("Successfully created a student", Severity.Success);
            _nav.NavigateTo($"/classrooms/{ClassroomId}");
            return;
        }

        _snackbar.Add($"Failed to create classroom: {res.Problem.Title}", Severity.Error);
    }
}
