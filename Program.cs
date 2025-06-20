var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgWeb();

var dbProductEntitiesDb = postgres.AddDatabase("db-product-entities");

var backendProductsProject = builder.AddProject<Projects.Products_Backend_Api>("backend-products")
    .WithReference(dbProductEntitiesDb)
    .WaitFor(dbProductEntitiesDb);

var frontendHostProject = builder.AddProject<Projects.Products_Frontend_Host>("frontend-apphost")
    .WaitFor(backendProductsProject);

builder.AddYarp("ingress")
    .WithReference(backendProductsProject)
    .WithReference(frontendHostProject)
    .LoadFromConfiguration("ReverseProxy")
    .WithHttpsEndpoint(targetPort: 443);

await builder.Build().RunAsync();