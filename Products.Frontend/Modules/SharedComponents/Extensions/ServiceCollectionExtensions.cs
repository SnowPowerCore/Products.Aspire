using Microsoft.Extensions.DependencyInjection;

namespace Products.Frontend.SharedComponents.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedComponents(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}