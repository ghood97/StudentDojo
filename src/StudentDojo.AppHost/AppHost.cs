var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.StudentDojo>("studentdojo");

builder.Build().Run();
