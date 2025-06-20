## Project description
This project is a test project for Products API.
It contains following components:
- Products.Backend.Api: main API project;
- Products.Frontend.Host: Blazor Web App for interacting with API;
- Products.PublicApi: shared library for common DTOs and other things;
- Products.ServiceDefaults: common services, telemetry and insights for each respective component;
- Products.Aspire: .NET Aspire project which has to be run in order to see integrations;
- Products.Backend.AspireYarpGateway: YARP integration with .NET Aspire for reverse proxy.
- Products.Backend.IntegrationTests: Integration tests project to test endpoints of Products.Backend.Api.

You need Docker or Podman (was tested on Podman) to run this project. This project was tested on Windows Machine. For Windows Machine you would need to have the Ubuntu WSL installed (can be done through Microsoft Store).
You can execute tests by running the following command (it's assumed you 'cd' into the project root dir)

```dotnet test .\Products.Backend.IntegrationTests\Products.Backend.IntegrationTests.csproj```.

(make sure you **don't** run .NET Aspire project at the same moment as tests)

Run the .NET Aspire project and use following urls to access different parts of this system:
- https://localhost/api/products/scalar/v1  Api definition provided by Scalar (recent alternative for Swagger)
- https://localhost/api/products/v1  Just a regular endpoint to see your current list of products (following the requirements doc) (you can access other GET endpoints right from the address panel)
- https://localhost/app/products  Blazor Web App to view/manipulate the data from the UI
