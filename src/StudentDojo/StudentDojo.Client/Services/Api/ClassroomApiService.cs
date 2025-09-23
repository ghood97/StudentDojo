using StudentDojo.Client.Extensions;
using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Services.Api;

public interface IClassroomApiService
{
    Task<ApiResponse<IEnumerable<ClassroomDto>>> GetClassroomsAsync();
    Task<ApiResponse<ClassroomDto>> GetClassroomByIdAsync(int id);
    Task<ApiResponse<ClassroomDto>> CreateClassroomAsync(ClassroomCreateDto createDto);
}

public class ClassroomApiService : IClassroomApiService
{
    private readonly IApiClient _client;

    public ClassroomApiService(IApiClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<IEnumerable<ClassroomDto>>> GetClassroomsAsync()
    {
        ApiResponse<IEnumerable<ClassroomDto>> res = await _client.GetAsync<IEnumerable<ClassroomDto>>($"api/classrooms");
        return res;
    }

    public async Task<ApiResponse<ClassroomDto>> GetClassroomByIdAsync(int id)
    {
        ApiResponse<ClassroomDto> res = await _client.GetAsync<ClassroomDto>($"api/classrooms/{id}");
        return res;
    }

    public async Task<ApiResponse<ClassroomDto>> CreateClassroomAsync(ClassroomCreateDto createDto)
    {
        ApiResponse<ClassroomDto> res = await _client.PostAsync<ClassroomCreateDto, ClassroomDto>("api/classrooms", createDto);
        return res;
    }
}
