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

## Features
- .NET Aspire
- FastEndpoints
- Dapper + Dapper.FastCrud
- Apizr + Refit
- .json Resource Files source generator
- validation
- YARP integration
- API integration tests
- Scalar (Swagger replacement)

## Prerequisites
You need the latest .NET 9 SDK, which can be either managed by Visual Studio IDE (or Rider), or installed manually from https://dot.net.

You need Docker or Podman (was tested on Podman) to be able to run this project correctly.

You may need to install dev certificate to be able to work with https. Use the ```dotnet dev-certs https --trust``` command.

## How to run and links
Run the .NET Aspire project (you can do the "Debug -> Start New Instance") and use following urls to access different parts of this system:
- https://localhost:17150  .NET Aspire Dashboard
- https://localhost/api/products/scalar/v1  Api definition provided by Scalar (recent alternative for Swagger)
- https://localhost/api/products/v1  Just a regular endpoint to see your current list of products (following the requirements doc) (you can access other GET endpoints right from the address panel)
- https://localhost/app/products  Blazor Web App to view/manipulate the data from the UI

## Testing environment
This project was tested on Windows Machine. For Windows Machine you would need to have the Ubuntu WSL installed (can be done through Microsoft Store).

This project was tested on Windows 11 24H2, using Microsoft Edge (Chromium).

Visual Studio Code was used for the development and testing of this project.

## Integration tests
You can execute tests by running the following command (it's assumed you 'cd' into the project root dir)

```dotnet test .\Products.Backend.IntegrationTests\Products.Backend.IntegrationTests.csproj```.

(make sure you **don't** run .NET Aspire project at the same moment as tests)
