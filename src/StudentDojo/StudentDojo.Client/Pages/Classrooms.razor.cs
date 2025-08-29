using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentDojo.Client.Contracts.DataTransfer;
using StudentDojo.Client.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentDojo.Client.Pages;
public partial class Classrooms : ComponentBase
{
    private readonly IClassroomApiService _classroomService;
    private readonly ISnackbar _snackbar;

    private IEnumerable<ClassroomDto> _classrooms { get; set; } = [];
    private bool _isLoading = true;

    public Classrooms(IClassroomApiService classroomService, ISnackbar snackbar)
    {
        _classroomService = classroomService;
        _snackbar = snackbar;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var response = await _classroomService.GetClassroomsAsync(1); // Example teacherId
            if (response.IsSuccess)
            {
                _classrooms = response.Data;
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
}
