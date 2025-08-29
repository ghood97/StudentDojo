using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace StudentDojo.Client.Services.Api;

public class ApiResponse<T>
{
    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public ProblemDetails Problem { get; set; } = new ProblemDetails()
    {
        Type = "UnknownError",
        Title = "An unknown error occurred.",
        Detail = "No error details were provided.",
        Status = 417,
    };
}
