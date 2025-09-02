using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;
using StudentDojo.Core.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentDojo.Client.Pages;
public partial class Classroom : ComponentBase
{
    private readonly IClassroomApiService _classroomService;
    private readonly ISnackbar _snackbar;
    private readonly INavService _nav;

    private ClassroomDto _classroom { get; set; } = new();
    private bool _isLoading = true;
    [Parameter] public int? Id { get; set; }

    public Classroom(IClassroomApiService classroomService, ISnackbar snackbar, INavService nav)
    {
        _classroomService = classroomService;
        _snackbar = snackbar;
        _nav = nav;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (Id == null)
            {
                _snackbar.Add("Classroom ID is required", Severity.Error);
                _nav.NavigateTo("/classrooms");
                return;
            }
            var response = await _classroomService.GetClassroomByIdAsync(Id.Value);
            if (response.IsSuccess)
            {
                _classroom = response.Data;
            }
            else
            {
                _snackbar.Add($"Failed to load classrooms: {response.Problem.Title}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            _snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void OnCreateClassroom()
    {
        _nav.NavigateTo("/classrooms/create");
    }
}
