using Products.Backend.Core.Entities.Product;
using Products.Backend.Core.Interfaces.Repositories;

namespace Products.Backend.Api.Interfaces.Repositories.Dapper;

public interface IProductRepository : IRepository<ProductEntity>
{
}