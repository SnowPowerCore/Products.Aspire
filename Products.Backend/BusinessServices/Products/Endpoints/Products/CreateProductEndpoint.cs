using System.Net;
using System.Net.Mime;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Products.Backend.Api.Interfaces.Services;
using Products.Backend.Infrastructure;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Constants;
using Products.PublicApi.Extensions;
using Products.PublicApi.Utilities.Api;
using Products.PublicApi.Validation.Dto;

namespace Products.Backend.Api.Endpoints.Products;

public class CreateProductEndpoint : Endpoint<ProductRequestDto, ApiResponse?>
{
    private readonly IProductService _product;

    public IOptions<JsonOptions> JsonOptions { get; set; }

    public override void Configure()
    {
        Post(string.Empty);
        Version(1);
        SerializerContext(CoreSerializationContext.Default);
        Validator<ProductRequestValidator>();
        AllowAnonymous();
        Description(b => b
            .WithTags(ApiTagConstants.Products)
            .Accepts<ProductRequestDto>(MediaTypeNames.Application.Json)
            .Produces<ApiResponseForOpenApi<ProductResponseDto>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .Produces<ApiResponse>((int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.Json)
            .ProducesProblemFE((int)HttpStatusCode.BadRequest));
    }

    public CreateProductEndpoint(IProductService product)
    {
        _product = product;
    }

    public override async Task HandleAsync(ProductRequestDto req, CancellationToken ct)
    {
        var result = await _product.CreateProductAsync(req, ct);

        await SendAsync(
            result?.ToApiResponse(serializerOptions: JsonOptions.Value.SerializerOptions),
            result?.ToStatusCode() ?? (int)HttpStatusCode.InternalServerError,
            ct);
    }
}