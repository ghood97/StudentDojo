using Microsoft.EntityFrameworkCore;
using StudentDojo.Core.Data;
using StudentDojo.Migrations;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<StudentDojoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentDojoDb"));
});

var host = builder.Build();
host.Run();
