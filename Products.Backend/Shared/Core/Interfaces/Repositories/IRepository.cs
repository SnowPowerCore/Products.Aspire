using Products.Backend.Core.Base;
using Products.Backend.Core.Interfaces.CompiledQueries;

namespace Products.Backend.Core.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : notnull, BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);

    Task<IEnumerable<TEntity>> GetAllByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default);

    Task<TEntity?> GetByIdAsync(long id, CancellationToken token = default);

    Task<TEntity?> GetOneByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default);

    Task<bool> AnyByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default);

    Task<long> CountByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default);

    Task<TEntity> AddOrUpdateAsync(TEntity entity, long? id = null, bool saveChange = true, CancellationToken token = default);

    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool saveChange = true, CancellationToken token = default);

    Task<bool> RemoveAsync(TEntity entity, bool saveChange = true, CancellationToken token = default);
}