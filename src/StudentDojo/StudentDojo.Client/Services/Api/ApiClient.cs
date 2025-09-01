using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StudentDojo.Client.Extensions;
using System.Net.Http.Json;

namespace StudentDojo.Client.Services.Api;

public interface IApiClient
{
    Task<ApiResponse<T>> GetAsync<T>(string endpoint);

    Task<ApiResponse<T>> PostAsync<T>(string endpoint, T? data);

    Task<ApiResponse<TReturn>> PostAsync<TRequest, TReturn>(string endpoint, TRequest? data);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(HttpClient client, ILogger<ApiClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            HttpResponseMessage res = await _client.GetAsync(endpoint);
            return await res.GetApiResponseAsync<T>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>()
            {
                IsSuccess = false,
                Problem = new ProblemDetails()
                {
                    Type = "Unknown Error Occurred",
                    Title = "An error occurred while retrieving the subscriptions.",
                    Detail = ex.Message,
                    Status = 500
                }
            };
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, T? data)
    {
        return await PostAsync<T, T>(endpoint, data);
    }

    public async Task<ApiResponse<TReturn>> PostAsync<TRequest, TReturn>(string endpoint, TRequest? data)
    {
        try
        {
            HttpResponseMessage res = await _client.PostAsJsonAsync(endpoint, data);
            return await res.GetApiResponseAsync<TReturn>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<TReturn>()
            {
                IsSuccess = false,
                Problem = new ProblemDetails()
                {
                    Type = "UnknownErrorOccurred",
                    Title = "An unknown error occurred",
                    Detail = ex.Message,
                    Status = 500
                }
            };
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, T? data)
    {
        return await PutAsync<T, T>(endpoint, data);
    }

    public async Task<ApiResponse<TReturn>> PutAsync<TRequest, TReturn>(string endpoint, TRequest? data)
    {
        try
        {
            HttpResponseMessage res = await _client.PutAsJsonAsync(endpoint, data);
            return await res.GetApiResponseAsync<TReturn>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<TReturn>()
            {
                IsSuccess = false,
                Problem = new ProblemDetails()
                {
                    Type = "UnknownErrorOccurred",
                    Title = "An unknown error occurred",
                    Detail = ex.Message,
                    Status = 500
                }
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
    {
        try
        {
            HttpResponseMessage res = await _client.DeleteAsync(endpoint);
            res.EnsureSuccessStatusCode();
            return new ApiResponse<bool>()
            {
                IsSuccess = true,
                Data = true
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<bool>()
            {
                IsSuccess = false,
                Problem = new ProblemDetails()
                {
                    Type = "UnknownErrorOccurred",
                    Title = "An unknown error occurred",
                    Detail = e.Message,
                    Status = 500
                }
            };
        }
    }
}
