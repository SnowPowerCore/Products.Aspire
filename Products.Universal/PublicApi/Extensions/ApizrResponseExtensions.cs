using System.Text.Json;
using Apizr;
using Products.PublicApi.Utilities.Api;

namespace Products.PublicApi.Extensions;

public static class ApizrResponseExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static T? ToData<T>(this IApizrResponse<ApiResponse> response, out List<string> errors, JsonSerializerOptions serializerOptions = null) where T : notnull
    {
        errors = [];

        if (!string.IsNullOrEmpty(response.ApiResponse?.Error?.Content))
        {
            var errorResponse = JsonSerializer.Deserialize<ApiResponse?>(
                response.ApiResponse.Error.Content, _serializerOptions);
            errors.AddRange(errorResponse?.Errors ?? []);
        }

        if (!response.IsSuccess && !response.Exception.Handled)
        {
            errors.Add(response.Exception.Message);
        }

        var data = response.Result;

        if (data?.Errors?.Count > 0)
        {
            errors.AddRange(data.Errors);
            return default;
        }

        if (data?.DataCount <= 0)
        {
            return default;
        }

        if (data is default(ApiResponse))
        {
            return default;
        }

        return data.Data!.Deserialize<T>(serializerOptions ?? _serializerOptions);
    }
}