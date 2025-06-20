using System.Text.Json;

namespace Products.PublicApi.Utilities.Api;

public sealed record ApiResponse(JsonDocument? Data = default, int DataCount = 0, int StatusCode = -1, IReadOnlyCollection<string>? Errors = default);