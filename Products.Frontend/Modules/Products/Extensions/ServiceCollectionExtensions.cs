using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using TimeWarp.State.Plus;
using TimeWarp.State.Plus.Extensions;

namespace Products.Frontend.ProductsApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProducts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient(typeof(IRequestPostProcessor<,>), typeof(PersistentStatePostProcessor<,>));
        serviceCollection.AddScoped<IPersistenceService, PersistenceService>();
        serviceCollection.AddTimeWarpStateRouting();
        return serviceCollection;
    }
}