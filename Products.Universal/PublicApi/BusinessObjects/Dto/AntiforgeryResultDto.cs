namespace Products.PublicApi.BusinessObjects.Dto;

public sealed record AntiforgeryResultDto(
    string? RequestToken,
    string? HeaderName);