using System.Data;
using System.Globalization;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.OpenTelemetry.Middleware;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing.Constraints;
using MinimalStepifiedSystem.Core.Extensions;
using Npgsql;
using NSwag;
using Oakton;
using Products.Backend.Api.Interfaces.Repositories.Dapper;
using Products.Backend.Api.Interfaces.Services;
using Products.Backend.Api.Repositories.Dapper;
using Products.Backend.Api.Services;
using Products.Backend.Infrastructure;
using Products.Backend.Infrastructure.Extensions;
using Products.Backend.Infrastructure.Processors;
using Products.Backend.Infrastructure.Utilities;
using Products.PublicApi.Extensions;
using Products.ServiceDefaults.Extensions;
using Scalar.AspNetCore;

var jsonStringEnumConverter = new JsonStringEnumConverter();

var builder = WebApplication.CreateSlimBuilder(args);
builder.Host.UseDefaultServiceProvider(static (c, options) =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});
builder.Host.ApplyOaktonExtensions();

builder.Services.Configure<MassTransitHostOptions>(static options =>
{
    options.WaitUntilStarted = true;
});

builder.Services.Configure<RouteOptions>(static options =>
{
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
});

builder.Services.Configure<JsonOptions>(static options =>
{
    options.SerializerOptions.SetJsonSerializationContext();
});

builder.Services.ConfigureHttpJsonOptions(static options =>
{
    options.SerializerOptions.SetJsonSerializationContext();
});

builder.Services.Configure<CookiePolicyOptions>(static options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.Configure<ForwardedHeadersOptions>(static options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.WebHost.UseKestrelHttpsConfiguration();
builder.AddServiceDefaults();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenTelemetry().ConnectBackendServices();
builder.Services.AddScoped<IDbConnection>(db => new NpgsqlConnection(
    builder.Configuration.GetConnectionString("db-product-entities")));
OrmExtensions.SetupDapper();

const int GlobalVersion = 1;
const string DefaultCulture = "en";
var supportedCultures = new[]
{
    new CultureInfo(DefaultCulture),
    new CultureInfo("ru")
};

builder.Services.AddAntiforgery(options => options.Cookie.Expiration = TimeSpan.Zero)
    .AddFastEndpoints(static options =>
    {
        options.SourceGeneratorDiscoveredTypes.AddRange(Products.Backend.Api.DiscoveredTypes.All);
    })
    .SwaggerDocument(options =>
    {
        options.AutoTagPathSegmentIndex = 0;
        options.ShortSchemaNames = true;
        options.MaxEndpointVersion = GlobalVersion;
        options.DocumentSettings = static s =>
        {
            s.DocumentName = $"v{GlobalVersion}";
            s.Version = $"v{GlobalVersion}";
            s.SchemaSettings.IgnoreObsoleteProperties = true;
            s.OperationProcessors.Add(new AntiforgeryHeaderProcessor());
        };
        options.SerializerSettings = s =>
        {
            s.Converters.Add(jsonStringEnumConverter);
            s.SetJsonSerializationContext();
            s.PropertyNamingPolicy = null;
            s.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        };
    });

builder.Services.AddScoped<IProductTableContextService, ProductTableContextService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseStepifiedSystem();
app.UseHttpsRedirection()
    .UseCookiePolicy(new()
    {
        MinimumSameSitePolicy = SameSiteMode.Strict,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    })
    .UseRequestLocalization(options =>
    {
        options.DefaultRequestCulture = new RequestCulture(DefaultCulture);
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders = [
            new AcceptLanguageHeaderRequestCultureProvider()
        ];
    })
    .UseAntiforgeryFE(additionalContentTypes: [MediaTypeNames.Application.Json])
    .UseFastEndpointsDiagnosticsMiddleware()
    .UseFastEndpoints(c =>
    {
        c.Endpoints.NameGenerator = static ctx =>
        {
            var currentName = ctx.EndpointType.Name;
            return currentName.TrimEnd("Endpoint");
        };
        c.Endpoints.ShortNames = true;
        c.Endpoints.RoutePrefix = default;
        c.Versioning.Prefix = "v";
        c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        c.Serializer.Options.Converters.Add(jsonStringEnumConverter);
        c.Serializer.Options.SetJsonSerializationContext();
        c.Serializer.ResponseSerializer = static (rsp, dto, contentType, _, cancellation) =>
        {
            if (dto is null)
                return Task.CompletedTask;
            rsp.ContentType = contentType;
            return rsp.WriteAsJsonAsync(
                value: dto,
                type: dto.GetType(),
                context: CoreSerializationContext.Default,
                cancellationToken: cancellation);
        };
        c.Errors.UseProblemDetails(static x =>
        {
            x.AllowDuplicateErrors = true;  //allows duplicate errors for the same error name
            x.IndicateErrorCode = true;     //serializes the fluentvalidation error code
            x.IndicateErrorSeverity = true; //serializes the fluentvalidation error severity
            x.TypeValue = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
            x.TitleValue = "One or more validation errors occurred.";
            x.TitleTransformer = static pd => pd.Status switch
            {
                400 => "Validation Error",
                404 => "Not Found",
                _ => "One or more errors occurred!"
            };
        });
        c.Errors.ResponseBuilder = static (failures, ctx, statusCode) =>
        {
            var failuresDict = failures
                .GroupBy(static f => f.PropertyName)
                .ToDictionary(
                    keySelector: static e => e.Key,
                    elementSelector: static e => e.Select(m => $"{e.Key}: {m.ErrorMessage}").ToArray());

            return ErrorResponseUtilities.ApiResponseWithErrors(
                failuresDict.Values.SelectMany(static x => x.Select(static s => s)).ToList(), statusCode);
        };
    });

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi(c =>
    {
        c.Path = "/openapi/{documentName}.json";
        c.PostProcess = (doc, req) =>
        {
            doc.Host = "https://localhost/api/products";
            doc.Schemes = [OpenApiSchema.Https];
        };
    });
    app.MapScalarApiReference(o =>
    {
        o.DarkMode = true;
    });
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<IProductTableContextService>();
await context.InitAsync();

await app.RunOaktonCommands(args);