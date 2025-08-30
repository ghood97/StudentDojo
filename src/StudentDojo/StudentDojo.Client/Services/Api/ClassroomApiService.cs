using StudentDojo.Client.Extensions;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Services.Api;

public interface IClassroomApiService
{
    Task<ApiResponse<IEnumerable<ClassroomDto>>> GetClassroomsAsync(int teacherId);
}

public class ClassroomApiService : IClassroomApiService
{
    private readonly HttpClient _client;

    public ClassroomApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<IEnumerable<ClassroomDto>>> GetClassroomsAsync(int teacherId)
    {
        HttpResponseMessage res = await _client.GetAsync($"api/classrooms/teacher/{teacherId}");
        return await res.GetApiResponseAsync<IEnumerable<ClassroomDto>>();
    }
}
