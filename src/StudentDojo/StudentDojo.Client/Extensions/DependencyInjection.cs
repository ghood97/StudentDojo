using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;

namespace StudentDojo.Client.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentDojoApiServices(this IServiceCollection services)
    {
        // Register application services here
        services.AddScoped<IClassroomApiService, ClassroomApiService>();
        services.AddScoped<IStudentApiService, StudentApiService>();
        return services;
    }

    public static IServiceCollection AddStudentDojoClientService(this IServiceCollection services)
    {
        services.AddScoped<INavService, NavService>();
        services.AddScoped<PointService>();
        return services;
    }
}
