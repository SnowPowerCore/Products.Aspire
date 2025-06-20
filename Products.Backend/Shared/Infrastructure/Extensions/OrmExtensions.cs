using System.ComponentModel.DataAnnotations.Schema;
using Dapper.FastCrud;
using Products.Backend.Core.Constants;
using Products.Backend.Core.Entities.Product;

namespace Products.Backend.Infrastructure.Extensions;

public class OrmExtensions
{
    public static void SetupDapper()
    {
        OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
        OrmConfiguration.RegisterEntity<ProductEntity>()
            .SetTableName(DatabaseConstants.ProductsTableName)
            .SetProperty(building => building.Id, 
                propMapping => propMapping.SetPrimaryKey()
                    .SetDatabaseGenerated(DatabaseGeneratedOption.Identity)
                    .SetDatabaseColumnName(nameof(ProductEntity.Id)))
            .SetProperty(building => building.Name,
                propMapping => propMapping.SetDatabaseColumnName(nameof(ProductEntity.Name)))
            .SetProperty(building => building.Price,
                propMapping => propMapping.SetDatabaseColumnName(nameof(ProductEntity.Price)))
            .SetProperty(building => building.Description,
                propMapping => propMapping.SetDatabaseColumnName(nameof(ProductEntity.Description)));
    }
}