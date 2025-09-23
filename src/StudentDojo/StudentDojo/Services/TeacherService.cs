using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using StudentDojo.Core.Data.Entities;
using StudentDojo.Core.DataTransfer;
using StudentDojo.Core.Extensions;
using System.Security.Claims;

namespace StudentDojo.Services;

public interface ITeacherService
{
    Task<TeacherDto> EnsureTeacherExistsAsync(ClaimsPrincipal principal);

    Task<TeacherDto> GetCurrentTeacherAsync();
}

public class TeacherService : ITeacherService
{
    private readonly StudentDojoDbContext _db;
    private readonly IHttpContextAccessor _accessor;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(StudentDojoDbContext db, IHttpContextAccessor accessor, ILogger<TeacherService> logger)
    {
        _db = db;
        _accessor = accessor;
        _logger = logger;
    }

    public async Task<TeacherDto> EnsureTeacherExistsAsync(ClaimsPrincipal principal)
    {
        string? authId = principal.GetAuthId();
        if (string.IsNullOrEmpty(authId))
        {
            throw new InvalidOperationException("No auth ID claim present");
        }

        Teacher? existing = await GetByAuthIdAsync(authId);
        if (existing != null)
        {
            return new TeacherDto(existing);
        }

        string? email = principal.GetEmail();
        string? firstName = principal.GetGivenName();
        string? lastName = principal.GetSurname();
        string? displayName = principal.GetName();

        if (email is null || firstName is null || lastName is null || displayName is null)
        {
            throw new InvalidOperationException("Missing required claim(s)");
        }

        Teacher newTeacher = new()
        {
            AuthId = authId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            DisplayName = displayName
        };
        _db.Teachers.Add(newTeacher);
        await _db.SaveChangesAsync();

        return new TeacherDto(newTeacher);
    }

    public async Task<TeacherDto> GetCurrentTeacherAsync()
    {
        ClaimsPrincipal currentUser = _accessor.HttpContext?.User ?? throw new InvalidOperationException("No HttpContext or User available");
        string authId = currentUser.GetAuthId() ?? throw new InvalidOperationException("No auth ID claim present");
        Teacher? teacher = await GetByAuthIdAsync(authId);
        if (teacher == null)
        {
            _logger.LogWarning("Teacher with AuthId {AuthId} not found", authId);
            throw new InvalidOperationException("Current teacher not found");
        }

        return new TeacherDto(teacher);
    }

    private async Task<Teacher?> GetByAuthIdAsync(string authId)
    {
        return await _db.Teachers.FirstOrDefaultAsync(t => t.AuthId == authId);
    }
}
