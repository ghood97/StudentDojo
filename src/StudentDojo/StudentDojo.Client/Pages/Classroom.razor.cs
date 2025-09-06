using Microsoft.AspNetCore.Components;
using MudBlazor;
using StudentDojo.Client.Components.Dialogs;
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
    private readonly IStudentApiService _studentService;
    private readonly ISnackbar _snackbar;
    private readonly IDialogService _dialogService;
    private readonly INavService _nav;
    private readonly PointHubService _pointHubService;

    private ClassroomDto _classroom { get; set; } = new();
    private bool _isLoading = true;
    private bool _isConnected = false;
    [Parameter] public int ClassroomId { get; set; }

    public Classroom(
        IClassroomApiService classroomService,
        IStudentApiService studentService,
        PointHubService pointService,
        ISnackbar snackbar,
        IDialogService dialogService,
        INavService nav)
    {
        _classroomService = classroomService;
        _studentService = studentService;
        _snackbar = snackbar;
        _dialogService = dialogService;
        _nav = nav;
        _pointHubService = pointService;
    }

    protected override async Task OnInitializedAsync()
    {
        ApiResponse<ClassroomDto> response = await _classroomService.GetClassroomByIdAsync(ClassroomId);
        if (response.IsSuccess)
        {
            _classroom = response.Data;
            _pointHubService.PointsUpdated += OnPointsUpdated;
            _pointHubService.ConnectionChanged += OnConnChanged;
            await _pointHubService.StartAsync();
            await _pointHubService.SubscribeToClassroomAsync(ClassroomId);
            _isConnected = true;
        }
        else
        {
            _snackbar.Add($"Failed to load classrooms: {response.Problem.Title}", Severity.Error);
        }

        _isLoading = false;
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
        _nav.NavigateTo($"/classrooms/{ClassroomId}/createStudent");
    }

    private void OnPointsUpdated(int studentId, int newPoints)
    {
        StudentDto? student = _classroom.Students.FirstOrDefault(s => s.Id == studentId);
        if (student is not null && student.Points != newPoints)
        {
            student.Points = newPoints;
            InvokeAsync(() => _snackbar.Add($"Points updated for {student.Name}. New Points {newPoints}", severity: Severity.Warning));
            InvokeAsync(StateHasChanged);
        }
    }

    private async Task IncrementStudentPointsAsync(int studentId)
    {
        ApiResponse<int> res = await _studentService.IncrementPointsAsync(ClassroomId, studentId, 1);
        if (res.IsSuccess)
        {
            OnPointsUpdated(studentId, res.Data);
        }
        else
        {
            await InvokeAsync(() => _snackbar.Add($"Failed to increment points: {res.Problem.Title}", Severity.Error));
        }
    }

    private async Task RedeemPointsAsync(int studentId)
    {
        IDialogReference dialog = await _dialogService.ShowAsync<RedeemPointsDialog>();
        DialogResult? result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            int pointsToRedeem = (int)result.Data!;
            ApiResponse<int> res = await _studentService.RedeemPointsAsync(ClassroomId, studentId, pointsToRedeem);
            if (res.IsSuccess)
            {
                OnPointsUpdated(studentId, res.Data);
                await InvokeAsync(() => _snackbar.Add($"Successfully redeemed {pointsToRedeem} points.", Severity.Success));
            }
            else
            {
                await InvokeAsync(() => _snackbar.Add($"Failed to redeem points: {res.Problem.Title}", Severity.Error));
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        _pointHubService.PointsUpdated -= OnPointsUpdated;
        _pointHubService.ConnectionChanged -= OnConnChanged;
        await _pointHubService.UnsubscribeFromClassroomAsync(ClassroomId);
        await _pointHubService.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
