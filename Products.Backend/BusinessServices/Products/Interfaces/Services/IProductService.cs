using MaybeResults;
using Products.PublicApi.BusinessObjects.Dto;

namespace Products.Backend.Api.Interfaces.Services;

public interface IProductService
{
    Task<IMaybe<List<ProductResponseDto>>> GetAllProductsAsync(CancellationToken token = default);

    Task<IMaybe<ProductResponseDto>> GetProductByIdAsync(long id, CancellationToken token = default);

    Task<IMaybe<ProductResponseDto>> CreateProductAsync(ProductRequestDto request, CancellationToken token = default);

    Task<IMaybe<ProductResponseDto>> UpdateProductAsync(long id, ProductRequestDto request, CancellationToken token = default);

    Task<IMaybe<ProductDeletedResponseDto>> DeleteProductAsync(long id, CancellationToken token = default);
}