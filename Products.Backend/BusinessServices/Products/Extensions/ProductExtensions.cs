using Products.Backend.Core.Entities.Product;
using Products.PublicApi.BusinessObjects.Dto;
using Riok.Mapperly.Abstractions;

namespace Products.Backend.ReadersManagement.Extensions;

[Mapper]
public static partial class ProductExtensions
{
    public static partial ProductResponseDto ToDto(this ProductEntity requestAssertionOptions);

    public static partial ProductEntity ToEntity(this ProductRequestDto requestAssertionOptions);
}