using Microsoft.AspNetCore.SignalR;
using StudentDojo.Services;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Hubs;

public class PointHub : Hub
{
    private readonly IStudentService _studentService;
    private readonly IClassroomService _classroomService;

    public PointHub(IStudentService studentService, IClassroomService classroomService)
    {
        _studentService = studentService;
        _classroomService = classroomService;
    }

    public async Task GetStudentPoints(int studentId)
    {
        var points = await _studentService.GetPointsAsync(studentId);
        await Clients.Caller.SendAsync("PointsUpdated", studentId, points);
    }

    public async Task IncrementStudentPoints(int studentId, int points)
    {
        int updatedPoints = await _studentService.IncrementPointsForStudentAsync(studentId, points);
        await Clients.All.SendAsync("PointsUpdated", studentId, updatedPoints);
    }

    public async Task RedeemStudentPoints(int studentId, int points)
    {
        int updatedPoints = await _studentService.RedeemPointsForStudentAsync(studentId, points);
        await Clients.All.SendAsync("PointsUpdated", studentId, updatedPoints);
    }

    public async Task GetClassroomPoints(int classroomId)
    {
        // Implementation to get points for all students in the classroom
        // This is a placeholder and should be replaced with actual logic
        ClassroomDto classroom = await _classroomService.GetClassroomByIdAsync(classroomId);
        foreach (StudentDto student in classroom.Students)
        {
            await Clients.Caller.SendAsync("PointsUpdated", student.Id, student.Points);
        }
    }

    public async Task IncrementClassroomPoints(int classroomId, int points)
    {
        // Implementation to increment points for all students in the classroom
        // This is a placeholder and should be replaced with actual logic
        ClassroomDto classroom = await _classroomService.GetClassroomByIdAsync(classroomId);
        foreach (StudentDto student in classroom.Students)
        {
            int updatedPoints = await _studentService.IncrementPointsForStudentAsync(student.Id, points);
            await Clients.All.SendAsync("PointsUpdated", student.Id, updatedPoints);
        }
    }
}
