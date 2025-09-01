using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using StudentDojo.Client.Extensions;
using StudentDojo.Client.Services;
using StudentDojo.Client.Services.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.HideTransitionDuration = 750;
    config.SnackbarConfiguration.ShowTransitionDuration = 750;
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
});

builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddStudentDojoApiServices();
builder.Services.AddStudentDojoClientService();

builder.Services.AddScoped<CounterService>();

await builder.Build().RunAsync();
