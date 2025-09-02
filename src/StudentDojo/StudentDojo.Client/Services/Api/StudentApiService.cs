using StudentDojo.Core.DataTransfer;

namespace StudentDojo.Client.Services.Api;

public interface IStudentApiService
{
    Task<ApiResponse<StudentDto>> CreateStudentAsync(StudentCreateDto createDto);
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
        ApiResponse<StudentDto> res = await _client.PostAsync<StudentCreateDto, StudentDto>("api/students", createDto);
        return res;
    }
}
