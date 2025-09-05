using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Services.Api;

public interface IStudentApiService
{
    Task<ApiResponse<StudentDto>> CreateStudentAsync(StudentCreateDto createDto);
    Task<ApiResponse<int>> IncrementPointsAsync(int classroomId, int studentId, int pointsDelta);
}

public class StudentApiService : IStudentApiService
{
    private readonly IApiClient _client;

    public StudentApiService(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<StudentDto>> CreateStudentAsync(StudentCreateDto createDto)
    {
        ApiResponse<StudentDto> res = await _client
            .PostAsync<StudentCreateDto, StudentDto>($"api/classrooms/{createDto.ClassroomId}/students", createDto);
        return res;
    }

    public async Task<ApiResponse<int>> IncrementPointsAsync(int classroomId, int studentId, int pointsDelta)
    {
        ApiResponse<int> res = await _client
            .PatchAsync<int, int>($"api/classrooms/{classroomId}/students/{studentId}/points", pointsDelta);
        return res;
    }
}
