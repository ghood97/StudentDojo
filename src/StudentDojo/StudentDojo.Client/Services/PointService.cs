using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace StudentDojo.Client.Services;

//public enum ConnectionEvent
//{
//    Closed,
//    Reconnecting,
//    Reconnected
//}

public class PointService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly INavService _nav;
    private int _classroomId;

    public PointService(INavService nav)
    {
        _nav = nav;
    }

    // If your hub sends studentId and points, update the event accordingly
    public event Action<int, int>? PointsUpdated;
    public event Action<ConnectionEvent>? ConnectionChanged;

    private HubConnection CreateHubConnection(int classroomId)
    {
        var connection = new HubConnectionBuilder()
        .WithUrl(_nav.ToAbsoluteUri("/pointHub"))
        .WithAutomaticReconnect()
        .Build();

        // server-to-client updates
        connection.On<int, int>("PointsUpdated", (studentId, points) => PointsUpdated?.Invoke(studentId, points));

        connection.Closed += error =>
        {
            ConnectionChanged?.Invoke(ConnectionEvent.Closed);
            return Task.CompletedTask;
        };

        connection.Reconnecting += error =>
        {
            ConnectionChanged?.Invoke(ConnectionEvent.Reconnecting);
            return Task.CompletedTask;
        };

        connection.Reconnected += async _ =>
        {
            ConnectionChanged?.Invoke(ConnectionEvent.Reconnected);
            await connection.SendAsync("GetClassroomPoints", classroomId);
        };

        return connection;
    }

    public async Task StartAsync(int classroomId)
    {
        _classroomId = classroomId;
        _hubConnection ??= CreateHubConnection(classroomId);

        if (_hubConnection.State is HubConnectionState.Connected or HubConnectionState.Connecting or HubConnectionState.Reconnecting)
        {
            return;
        }

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("GetClassroomPoints", classroomId);
    }

    // Optionally pass studentId if your hub requires it
    public async Task IncrementPointsAsync(int studentId)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("IncrementStudentPoints", studentId, 1);
        }
    }

    public async Task RedeemPointsAsync(int studentId, int points)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("RedeemStudentPoints", studentId, points);
        }
    }

    public async Task IncrementClassroomPoints()
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            // Assuming you have a method to get all student IDs in the classroom
            await _hubConnection.SendAsync("IncrementClassroomPoints", _classroomId, 1);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}