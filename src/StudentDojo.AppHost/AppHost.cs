var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> dbPassword = builder.AddParameter("StudentDojoSqlServerPassword", secret: true);

#pragma warning disable ASPIREPROXYENDPOINTS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
IResourceBuilder<SqlServerServerResource> sqlServer = builder
    .AddSqlServer("studentdojo-sql-server", dbPassword, 1433)
    .WithDataVolume("studentdojo-data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName("studentdojo.database")
    .WithEndpointProxySupport(false);
#pragma warning restore ASPIREPROXYENDPOINTS001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

IResourceBuilder<SqlServerDatabaseResource> database = sqlServer.AddDatabase("studentdojo-database", "studentdojo");

IResourceBuilder<ProjectResource> migrations = builder.AddProject<Projects.StudentDojo_Migrations>("migrations")
    .WithReference(database, connectionName: "StudentDojoDb")
    .WaitFor(database);

builder.AddProject<Projects.StudentDojo>("studentdojo")
    .WaitForCompletion(migrations)
    .WithReference(database)
    .WaitFor(database)
    .WithExternalHttpEndpoints();

builder.Build().Run();
