using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Refit;
using snowcoreBlog.Frontend.ClientShared.Handlers;
using snowcoreBlog.Frontend.ReadersManagement.Extensions;
using snowcoreBlog.Frontend.SharedComponents.Extensions;
using TimeWarp.State;

namespace snowcoreBlog.Frontend.ClientShared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClient(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddReadersManagement();
        serviceCollection.AddSharedComponents();
        serviceCollection.AddBlazoredLocalStorage();
        serviceCollection.AddBlazoredSessionStorage();
        serviceCollection.AddTimeWarpState(static options =>
        {
            options.Assemblies = [
                typeof(ReadersManagement.Extensions.ServiceCollectionExtensions).Assembly,
                typeof(SharedComponents.Extensions.ServiceCollectionExtensions).Assembly,
                typeof(TimeWarp.State.Plus.AssemblyMarker).Assembly
            ];
        });
        serviceCollection.ConfigureSnowcoreBlogBackendReadersManagementApizrManagers(options => options
            .WithBaseAddress("https://localhost/api/readers")
            .WithRefitSettings(new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(serializerOptions)
            })
            .WithRequestTimeout(TimeSpan.FromMinutes(1))
            .WithOperationTimeout(TimeSpan.FromMinutes(3))
            .WithHttpMessageHandler<IncludeCookiesHandler>());
        serviceCollection.AddFluentUIComponents();

        serviceCollection.AddAuthorizationCore();
        serviceCollection.AddCascadingAuthenticationState();
        serviceCollection.AddScoped<IncludeCookiesHandler>();
        
        serviceCollection.AddSingleton<IValidator<RequestCreateReaderAccountDto>, RequestCreateReaderAccountValidator>();
        serviceCollection.AddSingleton<IValidator<RequestAssertionOptionsDto>, RequestAssertionOptionsValidator>();
        serviceCollection.AddSingleton<IValidator<RequestAttestationOptionsDto>, RequestAttestationOptionsValidator>();
        serviceCollection.AddSingleton<IValidator<ConfirmCreateReaderAccountDto>, ConfirmCreateReaderAccountValidator>();
        
        return serviceCollection;
    }
}