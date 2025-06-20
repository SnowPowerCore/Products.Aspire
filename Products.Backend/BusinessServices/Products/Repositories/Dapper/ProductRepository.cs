using System.Data;
using Products.Backend.Api.Interfaces.Repositories.Dapper;
using Products.Backend.Core.Entities.Product;
using Products.Backend.Infrastructure.Repositories.Dapper.Base;

namespace Products.Backend.Api.Repositories.Dapper;

public class ProductRepository(IDbConnection conn) : BaseDapperRepository<ProductEntity>(conn), IProductRepository;