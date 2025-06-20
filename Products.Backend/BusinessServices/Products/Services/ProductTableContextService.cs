using System.Data;
using Dapper;
using Products.Backend.Api.Interfaces.Services;
using Products.Backend.Core.Constants;
using Products.Backend.Core.Entities.Product;

namespace Products.Backend.Api.Services;

public class ProductTableContextService : IProductTableContextService
{
    private readonly IDbConnection _conn;

    public ProductTableContextService(IDbConnection conn)
    {
        _conn = conn;
    }

    public async Task InitAsync()
    {
        var sql = $"""
            CREATE TABLE IF NOT EXISTS "{DatabaseConstants.ProductsTableName}" (
                "{nameof(ProductEntity.Id)}" SERIAL PRIMARY KEY,
                "{nameof(ProductEntity.Name)}" TEXT,
                "{nameof(ProductEntity.Price)}" NUMERIC(38,4),
                "{nameof(ProductEntity.Description)}" TEXT
            );
        """;
        await _conn.ExecuteAsync(sql);
    }
}