using StudentDojo.Client.Services.Api;

namespace StudentDojo.Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentDojoClientServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IClassroomApiService, ClassroomApiService>();
        return services;
    }
}
