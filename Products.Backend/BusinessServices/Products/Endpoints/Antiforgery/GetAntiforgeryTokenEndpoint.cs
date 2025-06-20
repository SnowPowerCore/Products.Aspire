using System.Net;
using FastEndpoints;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using MaybeResults;
using System.Net.Mime;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Utilities.Api;
using Products.PublicApi.Extensions;
using Products.PublicApi.Constants;
using Products.Backend.Infrastructure;

namespace Products.Backend.Api.Endpoints.Antiforgery;

public class GetAntiforgeryTokenEndpoint : EndpointWithoutRequest
{
    public IAntiforgery _antiforgery;

    public IOptions<JsonOptions> JsonOptions { get; set; }

    public override void Configure()
    {
        Get("antiforgerytoken");
        Version(1);
        SerializerContext(CoreSerializationContext.Default);
        AllowAnonymous();
        Description(b => b
            .WithTags(ApiTagConstants.Tokens)
            .Produces<ApiResponseForOpenApi<AntiforgeryResultDto>>((int)HttpStatusCode.OK, MediaTypeNames.Application.Json)
            .Produces<ApiResponse>((int)HttpStatusCode.InternalServerError, MediaTypeNames.Application.Json)
            .ProducesProblemFE((int)HttpStatusCode.BadRequest));
    }

    public GetAntiforgeryTokenEndpoint(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tokenSet = _antiforgery.GetAndStoreTokens(HttpContext);
        var result = Maybe.Create(new AntiforgeryResultDto(tokenSet.RequestToken, tokenSet.HeaderName));
        await SendAsync(
            result?.ToApiResponse(serializerOptions: JsonOptions.Value.SerializerOptions),
            result?.ToStatusCode() ?? (int)HttpStatusCode.InternalServerError,
            ct);
    }
}