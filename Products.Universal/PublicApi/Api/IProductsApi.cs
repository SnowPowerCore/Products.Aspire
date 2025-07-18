// <auto-generated>
//     This code was generated by Refitter.
// </auto-generated>


using Refit;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Apizr.Configuring.Request;
using System.Threading.Tasks;

using Products.PublicApi.Utilities.Api;
using Products.PublicApi.BusinessObjects.Dto;

#nullable enable annotations

namespace Products.PublicApi.Api
{
    [System.CodeDom.Compiler.GeneratedCode("Refitter", "1.5.5.0")]
    public partial interface IProductsApi
    {
        /// <param name="requestVerificationToken">A required antiforgery token that has to be sent along the request with implicit cookie as a pair.</param>
        /// <param name="options">The <see cref="IApizrRequestOptions"/> instance to pass through the request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the <see cref="IApiResponse"/> instance containing the result:
        /// <list type="table">
        /// <listheader>
        /// <term>Status</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>200</term>
        /// <description>Success</description>
        /// </item>
        /// <item>
        /// <term>400</term>
        /// <description>Bad Request</description>
        /// </item>
        /// <item>
        /// <term>500</term>
        /// <description>Server Error</description>
        /// </item>
        /// </list>
        /// </returns>
        [Headers("Accept: application/json, application/problem+json", "Content-Type: application/json")]
        [Post("/v1")]
        Task<IApiResponse<ApiResponse>> CreateProduct([Body, AliasAs("ProductRequestDto")] ProductRequestDto productRequestDto, [Header("RequestVerificationToken")] string requestVerificationToken, [RequestOptions] IApizrRequestOptions options);

        /// <param name="options">The <see cref="IApizrRequestOptions"/> instance to pass through the request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the <see cref="IApiResponse"/> instance containing the result:
        /// <list type="table">
        /// <listheader>
        /// <term>Status</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>200</term>
        /// <description>Success</description>
        /// </item>
        /// <item>
        /// <term>400</term>
        /// <description>Bad Request</description>
        /// </item>
        /// <item>
        /// <term>500</term>
        /// <description>Server Error</description>
        /// </item>
        /// </list>
        /// </returns>
        [Headers("Accept: application/json, application/problem+json")]
        [Get("/v1")]
        Task<IApiResponse<ApiResponse>> GetAllProducts([RequestOptions] IApizrRequestOptions options);

        /// <param name="requestVerificationToken">A required antiforgery token that has to be sent along the request with implicit cookie as a pair.</param>
        /// <param name="options">The <see cref="IApizrRequestOptions"/> instance to pass through the request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the <see cref="IApiResponse"/> instance containing the result:
        /// <list type="table">
        /// <listheader>
        /// <term>Status</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>200</term>
        /// <description>Success</description>
        /// </item>
        /// <item>
        /// <term>400</term>
        /// <description>Bad Request</description>
        /// </item>
        /// <item>
        /// <term>500</term>
        /// <description>Server Error</description>
        /// </item>
        /// </list>
        /// </returns>
        [Headers("Accept: application/json, application/problem+json")]
        [Delete("/{id}/v1")]
        Task<IApiResponse<ApiResponse>> DeleteProduct(string id, [Header("RequestVerificationToken")] string requestVerificationToken, [RequestOptions] IApizrRequestOptions options);

        /// <param name="options">The <see cref="IApizrRequestOptions"/> instance to pass through the request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the <see cref="IApiResponse"/> instance containing the result:
        /// <list type="table">
        /// <listheader>
        /// <term>Status</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>200</term>
        /// <description>Success</description>
        /// </item>
        /// <item>
        /// <term>400</term>
        /// <description>Bad Request</description>
        /// </item>
        /// <item>
        /// <term>500</term>
        /// <description>Server Error</description>
        /// </item>
        /// </list>
        /// </returns>
        [Headers("Accept: application/json, application/problem+json")]
        [Get("/{id}/v1")]
        Task<IApiResponse<ApiResponse>> GetProductById(string id, [RequestOptions] IApizrRequestOptions options);

        /// <param name="requestVerificationToken">A required antiforgery token that has to be sent along the request with implicit cookie as a pair.</param>
        /// <param name="options">The <see cref="IApizrRequestOptions"/> instance to pass through the request.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the <see cref="IApiResponse"/> instance containing the result:
        /// <list type="table">
        /// <listheader>
        /// <term>Status</term>
        /// <description>Description</description>
        /// </listheader>
        /// <item>
        /// <term>200</term>
        /// <description>Success</description>
        /// </item>
        /// <item>
        /// <term>400</term>
        /// <description>Bad Request</description>
        /// </item>
        /// <item>
        /// <term>500</term>
        /// <description>Server Error</description>
        /// </item>
        /// </list>
        /// </returns>
        [Headers("Accept: application/json, application/problem+json", "Content-Type: application/json")]
        [Put("/{id}/v1")]
        Task<IApiResponse<ApiResponse>> UpdateProduct(string id, [Body, AliasAs("ProductRequestDto")] ProductRequestDto productRequestDto, [Header("RequestVerificationToken")] string requestVerificationToken, [RequestOptions] IApizrRequestOptions options);
    }

}