using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using StudentDojo.Client.Pages;
using StudentDojo.Components;
using StudentDojo.Data;
using StudentDojo.Extensions;
using StudentDojo.Hubs;
using StudentDojo.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add MudBlazor services
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.HideTransitionDuration = 750;
    config.SnackbarConfiguration.ShowTransitionDuration = 750;
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddDbContextFactory<StudentDojoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDojoDb"));
});

builder.Services.AddStudentDojoServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
    };
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAntiforgery();

app.MapHub<CounterHub>("/counterHub");

app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(StudentDojo.Client._Imports).Assembly);

app.Run();
