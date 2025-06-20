namespace Products.PublicApi.BusinessObjects.Dto;

public sealed record ProductRequestDto
{
    public required string Name { get; set; }

    public required decimal Price { get; set; } = 0m;

    public string Description { get; set; } = string.Empty;
}