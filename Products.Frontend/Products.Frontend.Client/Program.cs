using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Products.Frontend.ClientShared.Extensions;
using Products.Frontend.ClientShared.Handlers;

namespace Products.Frontend.Client;

public class Program
{
    private static Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.ConfigureContainer(new DefaultServiceProviderFactory(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        }));
        builder.Services
            .AddHttpClient(string.Empty, sp => sp.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<IncludeCookiesHandler>();
        builder.Services.AddClient();

        return builder.Build().RunAsync();
    }
}