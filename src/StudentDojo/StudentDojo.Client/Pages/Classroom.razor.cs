using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;
using StudentDojo.Core.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentDojo.Client.Pages;
public partial class Classroom : ComponentBase, IAsyncDisposable
{
    private readonly IClassroomApiService _classroomService;
    private readonly ISnackbar _snackbar;
    private readonly INavService _nav;
    private readonly PointService _pointService;

    private ClassroomDto _classroom { get; set; } = new();
    private bool _isLoading = true;
    private bool _isConnected = false;
    [Parameter] public int? Id { get; set; }

    public Classroom(
        IClassroomApiService classroomService,
        ISnackbar snackbar,
        INavService nav,
        PointService pointService)
    {
        _classroomService = classroomService;
        _snackbar = snackbar;
        _nav = nav;
        _pointService = pointService;
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

            _pointService.PointsUpdated += OnPointsUpdated;
            _pointService.ConnectionChanged += OnConnChanged;

            await _pointService.StartAsync(_classroom.Id);
            _isConnected = true;
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

    private void OnConnChanged(ConnectionEvent e)
    {
        switch (e)
        {
            case ConnectionEvent.Reconnecting:
                _isConnected = false;
                InvokeAsync(() => _snackbar.Add("Connection lost. Reconnecting...", Severity.Warning));
                break;
            case ConnectionEvent.Reconnected:
                _isConnected = true;
                InvokeAsync(() => _snackbar.Add("Reconnected.", Severity.Success));
                break;
            case ConnectionEvent.Closed:
                _isConnected = false;
                InvokeAsync(() => _snackbar.Add("Connection closed.", Severity.Error));
                break;
        }
        InvokeAsync(StateHasChanged);
    }

    private void OnCreateStudent()
    {
        _nav.NavigateTo($"/classrooms/{Id}/createStudent");
    }

    private void OnPointsUpdated(int studentId, int points)
    {
        //_counter = newCounter;
        StudentDto? student = _classroom.Students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
        {
            return;
        }
        student.Points = points;
        InvokeAsync(() => _snackbar.Add($"Points updated for {student.Name}. New Points {points}", severity: Severity.Warning));
        InvokeAsync(StateHasChanged);
    }

    private async Task IncrementStudentPointsAsync(int studentId)
    {
        try
        {
            await _pointService.IncrementPointsAsync(studentId);
        }
        catch (Exception ex)
        {
            await InvokeAsync(() => _snackbar.Add("An error occurred while incrementing the counter.", Severity.Error));
        }
    }

    public async ValueTask DisposeAsync()
    {
        _pointService.PointsUpdated -= OnPointsUpdated;
        _pointService.ConnectionChanged -= OnConnChanged;
        await _pointService.DisposeAsync();
    }
}
