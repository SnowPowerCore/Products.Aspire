namespace Products.PublicApi.BusinessObjects.Dto;

public sealed record ProductResponseDto(
    int Id,
    string Name = "",
    decimal Price = 0m,
    string Description = ""
);