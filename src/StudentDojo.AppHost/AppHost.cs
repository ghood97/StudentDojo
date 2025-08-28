var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.StudentDojo>("studentdojo")
    .WithExternalHttpEndpoints();

builder.Build().Run();
