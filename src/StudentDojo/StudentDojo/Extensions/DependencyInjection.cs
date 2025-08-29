using StudentDojo.Services;

namespace StudentDojo.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentDojoServices(this IServiceCollection services)
    {
        // Register application services here
         services.AddScoped<IClassroomService, ClassroomService>();
        return services;
    }
}
