using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentDojo.Client.Services.Api;
using System.Net;

namespace StudentDojo.Client.Extensions;

public static class UtilityExtensions
{
    public static async Task<ApiResponse<T>> GetApiResponseAsync<T>(this HttpResponseMessage response)
    {
        static ProblemDetails CreateProblem(string type, string title, string detail, HttpResponseMessage resp)
        {
            return new()
            {
                Type = type,
                Title = title,
                Detail = detail,
                Status = (int)resp.StatusCode,
                Instance = resp.RequestMessage?.RequestUri?.ToString()
            };
        }

        string content = await response.Content.ReadAsStringAsync();

        // If successful...
        if (response.IsSuccessStatusCode)
        {
            try
            {
                // Try deserializing the content into T
                T? data = JsonConvert.DeserializeObject<T>(content);
                if (data == null)
                {
                    // If data is null, consider this a type mismatch or empty payload
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Problem = CreateProblem(
                            "InvalidResponse",
                            "Response deserilaization error",
                            "The response content could not be deserialized into the requested type.",
                            response)
                    };
                }

                return new ApiResponse<T>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (JsonException)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    Problem = CreateProblem(
                        "InvalidResponseType",
                        "Response deserilaization error",
                        "Operation was successful but there was an issue processing the response",
                        response)
                };
            }
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    Problem = CreateProblem(
                        "NotFound",
                        "Resource not found",
                        "The requested resource was not found.",
                        response)
                };
            }

            ProblemDetails? customError = null;
            try
            {
                customError = JsonConvert.DeserializeObject<ProblemDetails>(content);
            }
            catch (JsonException jsonEx)
            {
                customError = CreateProblem(
                    "InvalidErrorFormat",
                    "Error deserializing error response",
                    $"Error deserializing error content: {jsonEx.Message}",
                    response);
            }

            return new ApiResponse<T>
            {
                IsSuccess = false,
                Problem = customError ?? CreateProblem(
                    "UnknownError",
                    "An unknown error occurred.",
                    "No error details were provided.",
                    response)
            };
        }
    }
}
