using Dapper;
using Npgsql;

namespace Products.Backend.IntegrationTests;

[Collection("PostgresTestCollection")]
public class DapperIntegrationTests
{
    private readonly PostgresContainerFixture _fixture;

    public DapperIntegrationTests(PostgresContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Can_Connect_And_Execute_Simple_Query()
    {
        await using var conn = new NpgsqlConnection(_fixture.ConnectionString);
        await conn.OpenAsync();
        var result = await conn.ExecuteScalarAsync<int>("SELECT 1;");
        Assert.Equal(1, result);
    }
}
