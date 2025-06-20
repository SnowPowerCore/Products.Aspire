using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Products.Backend.AspireYarpGateway.Middleware;

public class ExtraProtectionMiddleware : IMiddleware
{
    private const string XContentTypeOptionsHeaderValue = "nosniff";
    private const string XXSSProtectionHeaderValue = "1";
    private const string XFrameOptionsHeaderValue = "DENY";

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.Headers.Append(HeaderNames.XContentTypeOptions, XContentTypeOptionsHeaderValue);
        context.Response.Headers.Append(HeaderNames.XXSSProtection, XXSSProtectionHeaderValue);
        context.Response.Headers.Append(HeaderNames.XFrameOptions, XFrameOptionsHeaderValue);

        return next(context);
    }
}