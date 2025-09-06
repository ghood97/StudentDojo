using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace StudentDojo.Controllers;

public class BaseController : ControllerBase
{
    protected ObjectResult NotFoundProblem(string? title = null, string? detail = null)
        => CreateProblem(404, title ?? "Resource not found", detail);

    protected ObjectResult BadRequestProblem(string title, string? detail = null)
        => CreateProblem(400, title, detail);

    protected ObjectResult CreateProblem(int status, string title, string? detail = null)
    {
        var factory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var pd = factory.CreateProblemDetails(HttpContext, statusCode: status, title: title, detail: detail);
        return new ObjectResult(pd) { StatusCode = status };
    }
}
