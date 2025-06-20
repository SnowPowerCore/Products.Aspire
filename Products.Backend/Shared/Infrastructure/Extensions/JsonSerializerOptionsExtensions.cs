using System.Text.Json;

namespace Products.Backend.Infrastructure.Extensions;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions SetJsonSerializationContext(this JsonSerializerOptions options)
    {
        options.TypeInfoResolverChain.Insert(0, CoreSerializationContext.Default);
        return options;
    }
}