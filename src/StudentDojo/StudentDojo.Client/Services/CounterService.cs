using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace StudentDojo.Client.Services;

public class CounterService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;
    private bool _isStarted = false;

    public CounterService(IConfiguration configuration, NavigationManager nav)
    {
        var baseUrl = configuration.GetValue<string>("BaseUrl") ?? "https://localhost:7045";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(nav.ToAbsoluteUri("/counterHub"))
            .Build();

        _hubConnection.On<int>("CounterUpdated", (counter) =>
        {
            CounterUpdated?.Invoke(counter);
        });
    }

    public event Action<int>? CounterUpdated;

    public async Task StartAsync()
    {
        if (!_isStarted)
        {
            await _hubConnection.StartAsync();
            _isStarted = true;
            await _hubConnection.SendAsync("GetCurrentCounter");
        }
    }

    public async Task IncrementCounterAsync()
    {
        if (_isStarted)
        {
            await _hubConnection.SendAsync("IncrementCounter");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
