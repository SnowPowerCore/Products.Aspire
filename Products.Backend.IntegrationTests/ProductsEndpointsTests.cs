using System.Net.Http.Json;
using System.Text.Json;
using Products.PublicApi.BusinessObjects.Dto;
using Products.PublicApi.Utilities.Api;

namespace Products.Backend.IntegrationTests;

[Collection("PostgresTestCollection")]
public class ProductsEndpointsTests
{
    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly CustomWebApplicationFactory _factory;
    private readonly PostgresContainerFixture _fixture;

    public ProductsEndpointsTests(PostgresContainerFixture fixture)
    {
        _fixture = fixture;
        _factory = new CustomWebApplicationFactory(_fixture.ConnectionString);
    }

    [Fact]
    public async Task Create_And_Get_Product_Works()
    {
        var client = _factory.CreateClient();
        var createDto = new ProductRequestDto { Name = "Test Product", Price = 42.5m, Description = "desc" };
        var createResp = await client.PostAsJsonAsync("/v1", createDto);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.NotNull(created?.Data);
        var createdProduct = created.Data is not null ? JsonSerializer.Deserialize<ProductResponseDto>(created.Data.RootElement.GetRawText(), _serializerOptions) : null;
        Assert.NotNull(createdProduct);
        var id = createdProduct.Id;

        var getResp = await client.GetAsync($"/{id}/v1");
        getResp.EnsureSuccessStatusCode();
        var get = await getResp.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.NotNull(get?.Data);
        var getProduct = get.Data is not null ? JsonSerializer.Deserialize<ProductResponseDto>(get.Data.RootElement.GetRawText(), _serializerOptions) : null;
        Assert.NotNull(getProduct);
        Assert.Equal("Test Product", getProduct.Name);
    }

    [Fact]
    public async Task GetAllProducts_Returns_List()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/v1");
        resp.EnsureSuccessStatusCode();
        var list = await resp.Content.ReadFromJsonAsync<ApiResponse>();
        Assert.NotNull(list?.Data);
        var products = list.Data is not null ? JsonSerializer.Deserialize<List<ProductResponseDto>>(list.Data.RootElement.GetRawText(), _serializerOptions) : null;
        Assert.NotNull(products);
    }

    [Fact]
    public async Task UpdateProduct_Works()
    {
        var client = _factory.CreateClient();
        var createDto = new ProductRequestDto { Name = "ToUpdate", Price = 1, Description = "desc" };
        var createResp = await client.PostAsJsonAsync("/v1", createDto);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<ApiResponse>();
        var createdProduct = created!.Data is not null ? JsonSerializer.Deserialize<ProductResponseDto>(created.Data.RootElement.GetRawText(), _serializerOptions) : null;
        var id = createdProduct!.Id;

        var updateDto = new ProductRequestDto { Name = "Updated", Price = 2, Description = "desc2" };
        var updateResp = await client.PutAsJsonAsync($"/{id}/v1", updateDto);
        updateResp.EnsureSuccessStatusCode();
        var updated = await updateResp.Content.ReadFromJsonAsync<ApiResponse>();
        var updatedProduct = updated!.Data is not null ? JsonSerializer.Deserialize<ProductResponseDto>(updated.Data.RootElement.GetRawText(), _serializerOptions) : null;
        Assert.Equal("Updated", updatedProduct!.Name);
    }

    [Fact]
    public async Task DeleteProduct_Works()
    {
        var client = _factory.CreateClient();
        var createDto = new ProductRequestDto { Name = "ToDelete", Price = 1, Description = "desc" };
        var createResp = await client.PostAsJsonAsync("/v1", createDto);
        createResp.EnsureSuccessStatusCode();
        var created = await createResp.Content.ReadFromJsonAsync<ApiResponse>();
        var createdProduct = created!.Data is not null ? JsonSerializer.Deserialize<ProductResponseDto>(created.Data.RootElement.GetRawText(), _serializerOptions) : null;
        var id = createdProduct!.Id;

        var deleteResp = await client.DeleteAsync($"/{id}/v1");
        deleteResp.EnsureSuccessStatusCode();
        var deleted = await deleteResp.Content.ReadFromJsonAsync<ApiResponse>();
        var deletedResult = deleted!.Data is not null ? JsonSerializer.Deserialize<ProductDeletedResponseDto>(deleted.Data.RootElement.GetRawText(), _serializerOptions) : null;
        Assert.True(deletedResult!.Deleted);
    }
}
