using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using System.Diagnostics;

namespace StudentDojo.Migrations;

public class Worker : BackgroundService
{
    public const string ACTIVITY_SOURCE_NAME = "Migrations";
    private static readonly ActivitySource _activitySource = new(ACTIVITY_SOURCE_NAME);
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using Activity? activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            using StudentDojoDbContext dbContext = scope.ServiceProvider.GetRequiredService<StudentDojoDbContext>();

            await RunMigrationAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(StudentDojoDbContext dbContext, CancellationToken cancellationToken)
    {
        Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }
}
