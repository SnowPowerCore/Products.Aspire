using MaybeResults;
using Products.Backend.Api.Interfaces.Repositories.Dapper;
using Products.Backend.Api.Interfaces.Services;
using Products.Backend.Api.ErrorResults;
using Products.Backend.Core.Entities.Product;
using Products.Backend.ReadersManagement.Extensions;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Resources;

namespace Products.Backend.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IMaybe<List<ProductResponseDto>>> GetAllProductsAsync(CancellationToken token = default)
    {
        var productEntities = await _productRepository.GetAllAsync(token);
        if (productEntities?.Any() is not true)
        {
            return Maybe.Create(new List<ProductResponseDto>());
        }
        return Maybe.Create(productEntities.Select(x => x.ToDto()).ToList());
    }

    public async Task<IMaybe<ProductResponseDto>> GetProductByIdAsync(long id, CancellationToken token = default)
    {
        var productEntity = await _productRepository.GetByIdAsync(id, token);
        if (productEntity is default(ProductEntity))
        {
            return ProductNotFoundError<ProductResponseDto>.Create(
                string.Format(TranslationResources.ProductNotFoundErrorMessage, id));
        }
        return Maybe.Create(productEntity.ToDto());
    }

    public async Task<IMaybe<ProductResponseDto>> CreateProductAsync(ProductRequestDto request, CancellationToken token = default)
    {
        var productEntity = await _productRepository.AddOrUpdateAsync(request.ToEntity(), token: token);
        if (productEntity is default(ProductEntity))
        {
            return UnableToCreateProductError<ProductResponseDto>.Create(
                TranslationResources.UnableToCreateProductErrorMessage);
        }
        return Maybe.Create(productEntity.ToDto());
    }

    public async Task<IMaybe<ProductResponseDto>> UpdateProductAsync(long id, ProductRequestDto request, CancellationToken token = default)
    {
        var existingProductEntity = await _productRepository.GetByIdAsync(id, token);
        if (existingProductEntity is default(ProductEntity))
        {
            return ProductNotFoundError<ProductResponseDto>.Create(
                string.Format(TranslationResources.ProductNotFoundErrorMessage, id));
        }
        var productEntity = await _productRepository.AddOrUpdateAsync(existingProductEntity, id, token: token);
        if (productEntity is default(ProductEntity))
        {
            return UnableToUpdateProductError<ProductResponseDto>.Create(
                string.Format(TranslationResources.UnableToUpdateProductErrorMessage, id));
        }
        return Maybe.Create(productEntity.ToDto());
    }

    public async Task<IMaybe<ProductDeletedResponseDto>> DeleteProductAsync(long id, CancellationToken token = default)
    {
        var existingProductEntity = await _productRepository.GetByIdAsync(id, token);
        if (existingProductEntity is default(ProductEntity))
        {
            return Maybe.Create<ProductDeletedResponseDto>(new(false));
        }
        return Maybe.Create<ProductDeletedResponseDto>(
            new(await _productRepository.RemoveAsync(existingProductEntity, token: token)));
    }
}