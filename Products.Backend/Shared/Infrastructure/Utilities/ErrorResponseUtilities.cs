using Products.PublicApi.Utilities.Api;

namespace Products.Backend.Infrastructure.Utilities;

public static class ErrorResponseUtilities
{
    public static ApiResponse ApiResponseWithErrors(IReadOnlyCollection<string> errors, int statusCode) =>
        new(default, 0, statusCode, errors);
}