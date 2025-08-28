using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using StudentDojo.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.HideTransitionDuration = 750;
    config.SnackbarConfiguration.ShowTransitionDuration = 750;
    config.SnackbarConfiguration.VisibleStateDuration = 2500;
});

builder.Services.AddScoped<CounterService>();

await builder.Build().RunAsync();
