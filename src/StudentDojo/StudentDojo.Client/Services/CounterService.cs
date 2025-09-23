using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace StudentDojo.Client.Services;

public enum ConnectionEvent
{
    Closed,
    Reconnecting,
    Reconnected
}

public class CounterService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly INavService _nav;

    public CounterService(INavService nav)
    {
        _nav = nav;
    }

    public event Action<int>? CounterUpdated;
    public event Action<ConnectionEvent>? ConnectionChanged;

    private HubConnection CreateHubConnection()
    {
        var connection = new HubConnectionBuilder()
        .WithUrl(_nav.ToAbsoluteUri("/counterHub"))
        .WithAutomaticReconnect()
        .Build();

        // server-to-client updates
        connection.On<int>("CounterUpdated", c => CounterUpdated?.Invoke(c));

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
            await connection.SendAsync("GetCurrentCounter");
        };

        return connection;
    }

    public async Task StartAsync()
    {
        _hubConnection ??= CreateHubConnection();

        if (_hubConnection.State is HubConnectionState.Connected or HubConnectionState.Connecting or HubConnectionState.Reconnecting)
        {
            return;
        }

        await _hubConnection.StartAsync();
        await _hubConnection.SendAsync("GetCurrentCounter");
    }

    public async Task IncrementCounterAsync()
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("IncrementCounter");
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