using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Pages;
public partial class ClassroomCreate : ComponentBase
{
    private readonly ISnackbar _snackbar;
    private readonly IClassroomApiService _classroomService;
    private readonly INavService _nav;
    private ClassroomCreateDto _createDto = new();

    public ClassroomCreate(ISnackbar snackbar, IClassroomApiService classroomService, INavService nav)
    {
        _snackbar = snackbar;
        _classroomService = classroomService;
        _nav = nav;
    }

    private async Task OnValidSubmitAsync(EditContext context)
    {
        ApiResponse<ClassroomDto> res = await _classroomService.CreateClassroomAsync(_createDto);
        if (res.IsSuccess)
        {
            _snackbar.Add("Successfully created a classroom", Severity.Success);
            _nav.NavigateTo("/classrooms");
            return;
        }

        _snackbar.Add($"Failed to create classroom: {res.Problem.Title}", Severity.Error);
    }
}
