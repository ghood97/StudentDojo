using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace StudentDojo.Client.Services;

public class CounterService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private bool _isStarted = false;

    public CounterService(IConfiguration configuration, NavigationManager nav, ISnackbar snackbar)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(nav.ToAbsoluteUri("/counterHub"), options =>
            {
                // Configure transports for better compatibility
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets |
                                   Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
                options.SkipNegotiation = false;
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10) })
            .Build();

        _hubConnection.On<int>("CounterUpdated", (counter) =>
        {
            CounterUpdated?.Invoke(counter);
        });
        _hubConnection.Closed += async (error) =>
        {
            snackbar.Add("Connection closed. Attempting to reconnect...", Severity.Error);
        };

        _hubConnection.Reconnecting += async (error) =>
        {
            snackbar.Add("Connection lost. Attempting to reconnect...", Severity.Warning);
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            snackbar.Add("Reconnected to the server.", Severity.Success);
            await _hubConnection.SendAsync("GetCurrentCounter");
        };
    }

    public event Action<int>? CounterUpdated;

    public async Task StartAsync()
    {
        if (!_isStarted && _hubConnection.State == HubConnectionState.Disconnected)
        {
            await _hubConnection.StartAsync();
            _isStarted = true;
            await _hubConnection.SendAsync("GetCurrentCounter");
        }
    }

    public async Task IncrementCounterAsync()
    {
        if (_isStarted && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("IncrementCounter");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            _isStarted = false;
            await _hubConnection.DisposeAsync();
        }
    }
}