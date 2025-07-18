﻿using System.Net;
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

namespace Products.Backend.Api.Endpoints.Products;

public class GetAllProductsEndpoint : EndpointWithoutRequest<ApiResponse?>
{
    private readonly IProductService _product;

    public IOptions<JsonOptions> JsonOptions { get; set; }

    public override void Configure()
    {
        Get(string.Empty);
        Version(1);
        SerializerContext(CoreSerializationContext.Default);
        AllowAnonymous();
        Description(b => b
            .WithTags(ApiTagConstants.Products)
            .Produces<ApiResponseForOpenApi<List<ProductResponseDto>>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .Produces<ApiResponse>((int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.Json)
            .ProducesProblemFE((int)HttpStatusCode.BadRequest));
    }

    public GetAllProductsEndpoint(IProductService product)
    {
        _product = product;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _product.GetAllProductsAsync(ct);

        await SendAsync(
            result?.ToApiResponse(serializerOptions: JsonOptions.Value.SerializerOptions),
            result?.ToStatusCode() ?? (int)HttpStatusCode.InternalServerError,
            ct);
    }
}