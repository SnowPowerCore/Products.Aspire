using Products.Backend.Core.Base;

namespace Products.Backend.Core.Entities.Product;

public record ProductEntity : BaseEntity
{
    public string Name { get; init; }

    public decimal Price { get; init; } = 0m;

    public string Description { get; init; } = string.Empty;
}