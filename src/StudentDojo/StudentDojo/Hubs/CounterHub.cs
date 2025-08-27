using Microsoft.AspNetCore.SignalR;

namespace StudentDojo.Hubs;

public class CounterHub : Hub
{
    private static int _globalCounter = 0;

    public async Task IncrementCounter()
    {
        Interlocked.Increment(ref _globalCounter);
        await Clients.All.SendAsync("CounterUpdated", _globalCounter);
    }

    public async Task GetCurrentCounter()
    {
        await Clients.Caller.SendAsync("CounterUpdated", _globalCounter);
    }
}
