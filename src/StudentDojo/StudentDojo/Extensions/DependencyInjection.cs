using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using StudentDojo.Authentication;
using StudentDojo.Services;
using System.Security.Claims;

namespace StudentDojo.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentDojoServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IClassroomService, ClassroomService>();
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }

    public static IServiceCollection AddGoogleAuth(this IServiceCollection services, ConfigurationManager config)
    {
        GoogleAuthOptions googleOptions = config
            .GetSection("Authentication:Google")
            .Get<GoogleAuthOptions>()
            ?? throw new InvalidOperationException("Google auth config missing");


        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
        })
        .AddGoogle(options =>
        {
            options.ClientId = googleOptions.ClientId;
            options.ClientSecret = googleOptions.ClientSecret;

            // Helpful claim mappings
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

            options.SaveTokens = true;

            options.Events.OnTicketReceived = async (ctx) =>
            {
                ITeacherService teacherService = ctx.HttpContext.RequestServices.GetRequiredService<ITeacherService>();
                try
                {
                    await teacherService.EnsureTeacherExistsAsync(ctx.Principal!);
                }
                catch(Exception e)
                {
                    ctx.HttpContext.Response.Redirect($"/error?message={Uri.EscapeDataString(e.Message)}");
                    ctx.HandleResponse();
                }
            };
        });

        return services;
    }
}
