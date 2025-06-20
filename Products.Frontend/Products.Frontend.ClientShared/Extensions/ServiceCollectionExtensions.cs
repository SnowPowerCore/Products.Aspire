using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Products.Frontend.ClientShared.Handlers;
using Products.Frontend.Infrastructure.Providers;
using Products.Frontend.ProductsApi.Extensions;
using Products.Frontend.SharedComponents.Extensions;
using Products.PublicApi.Api;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Validation.Dto;
using Refit;
using TimeWarp.State;

namespace Products.Frontend.ClientShared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddProducts();
        serviceCollection.AddSharedComponents();
        serviceCollection.AddBlazoredLocalStorage();
        serviceCollection.AddBlazoredSessionStorage();
        serviceCollection.AddTimeWarpState(static options =>
        {
            options.Assemblies = [
                typeof(ProductsApi.Extensions.ServiceCollectionExtensions).Assembly,
                typeof(SharedComponents.Extensions.ServiceCollectionExtensions).Assembly,
                typeof(TimeWarp.State.Plus.AssemblyMarker).Assembly
            ];
        });
        serviceCollection.ConfigureProductsBackendApiApizrManagers(options => options
            .WithBaseAddress("https://localhost/api/products")
            .WithRefitSettings(new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer()
            })
            .WithRequestTimeout(TimeSpan.FromMinutes(1))
            .WithOperationTimeout(TimeSpan.FromMinutes(3))
            .WithHttpMessageHandler<IncludeCookiesHandler>());
        serviceCollection.AddFluentUIComponents();

        serviceCollection.AddAuthorizationCore();
        serviceCollection.AddCascadingAuthenticationState();
        serviceCollection.AddScoped<AuthenticationStateProvider, ProductsAuthStateProvider>();
        serviceCollection.AddScoped<IncludeCookiesHandler>();
        
        serviceCollection.AddSingleton<IValidator<ProductRequestDto>, ProductRequestValidator>();
        
        return serviceCollection;
    }
}