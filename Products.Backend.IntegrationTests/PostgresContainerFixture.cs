using Testcontainers.PostgreSql;

namespace Products.Backend.IntegrationTests;

public class PostgresContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; } = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}

[CollectionDefinition("PostgresTestCollection")]
public class PostgresTestCollection : ICollectionFixture<PostgresContainerFixture> { }
