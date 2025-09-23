using Microsoft.AspNetCore.SignalR;
using StudentDojo.Services;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Hubs;

public class PointHub : Hub
{
    public Task SubscribeToClassroom(int classroomId) => Groups.AddToGroupAsync(Context.ConnectionId, $"Classroom-{classroomId}");

    public Task UnsubscribeFromClassroom(int classroomId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Classroom-{classroomId}");
}
