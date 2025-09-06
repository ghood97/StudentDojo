using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace StudentDojo.Client.Services;

public class PointHubService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly INavService _nav;

    private readonly HashSet<int> _subscribedClassrooms = [];
    public PointHubService(INavService nav)
    {
        _nav = nav;
    }

    // If your hub sends studentId and points, update the event accordingly
    public event Action<int, int>? PointsUpdated;
    public event Action<ConnectionEvent>? ConnectionChanged;

    private HubConnection CreateHubConnection()
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

            foreach (int classroomId in _subscribedClassrooms)
            {
                try
                {
                    await connection.InvokeAsync("SubscribeToClassroom", classroomId);
                }
                catch
                {
                    // Ignore errors during reconnection attempts
                }
            }
        };

        return connection;
    }

    private async Task EnsureConnectedAsync()
    {
        _hubConnection ??= CreateHubConnection();

        if (_hubConnection.State is HubConnectionState.Disconnected)
        {
            await _hubConnection.StartAsync();
        }
    }

    public Task StartAsync() => EnsureConnectedAsync();

    public async Task SubscribeToClassroomAsync(int classroomId)
    {
        await EnsureConnectedAsync();

        await _hubConnection!.InvokeAsync("SubscribeToClassroom", classroomId);
        _subscribedClassrooms.Add(classroomId);
    }

    public async Task UnsubscribeFromClassroomAsync(int classroomId)
    {
        if (_hubConnection is null)
        {
            return;
        }

        await _hubConnection.InvokeAsync("UnsubscribeFromClassroom", classroomId);
        _subscribedClassrooms.Remove(classroomId);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
        _subscribedClassrooms.Clear();
        GC.SuppressFinalize(this);
    }
}