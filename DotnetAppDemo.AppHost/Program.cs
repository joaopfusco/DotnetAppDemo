using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Database
var postgres = builder.AddPostgres("DotnetAppDemoDB")
    .WithPgAdmin()
    .WithDataVolume();

// Keycloak
var keycloak = builder
    .AddKeycloak("keycloak", port: 18080)
    .WithDataVolume()
    .WithExternalHttpEndpoints();

// API
var api = builder.AddProject<Projects.DotnetAppDemo_API>("api")
    .WithReference(postgres)
    .WithReference(keycloak)
    .WithEnvironment("Keycloak__AuthorizationUrl", "http://localhost:18080/realms/DotnetAppDemo/protocol/openid-connect/auth")
    .WithEnvironment("Keycloak__TokenUrl", "http://localhost:18080/realms/DotnetAppDemo/protocol/openid-connect/token")
    .WithEnvironment("Keycloak__Audience", "account")
    .WithEnvironment("Keycloak__ClientId", "api-client")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.DotnetAppDemo_Web>("web")
    .WithReference(api)
    .WithEnvironment("Keycloak__AuthorizationUrl", "http://localhost:18080/realms/DotnetAppDemo/protocol/openid-connect/auth")
    .WithEnvironment("Keycloak__TokenUrl", "http://localhost:18080/realms/DotnetAppDemo/protocol/openid-connect/token")
    .WithEnvironment("Keycloak__Audience", "account")
    .WithEnvironment("Keycloak__ClientId", "api-client")
    .WithExternalHttpEndpoints();

builder.Build().Run();
