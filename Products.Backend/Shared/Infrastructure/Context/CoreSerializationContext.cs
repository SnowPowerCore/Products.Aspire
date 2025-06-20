using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using Products.Backend.Core.Entities.Product;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Utilities.Api;
using Scalar.AspNetCore;

namespace Products.Backend.Infrastructure;

/// <summary>
/// Defines the serialization context.
/// </summary>
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<byte>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(ScalarOptions))]
[JsonSerializable(typeof(List<ValidationFailure>))]
[JsonSerializable(typeof(ApiResponse))]
[JsonSerializable(typeof(ProductEntity))]
[JsonSerializable(typeof(ProductRequestDto))]
[JsonSerializable(typeof(ProductResponseDto))]
[JsonSerializable(typeof(List<ProductResponseDto>))]
[JsonSerializable(typeof(ProductDeletedResponseDto))]
[JsonSerializable(typeof(AntiforgeryResultDto))]
[JsonSourceGenerationOptions(
    JsonSerializerDefaults.Web,
    UseStringEnumConverter = true,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class CoreSerializationContext : JsonSerializerContext { }