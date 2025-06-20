using System.Data;
using Dapper;
using Dapper.FastCrud;
using Products.Backend.Core.Base;
using Products.Backend.Core.Interfaces.CompiledQueries;
using Products.Backend.Core.Interfaces.Repositories;

namespace Products.Backend.Infrastructure.Repositories.Dapper.Base;

public class BaseDapperRepository<TEntity> : IRepository<TEntity> where TEntity : notnull, BaseEntity, new()
{
    private readonly IDbConnection _conn;

    public BaseDapperRepository(IDbConnection conn)
    {
        _conn = conn;
    }

    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default) =>
        _conn.FindAsync<TEntity>();

    public Task<IEnumerable<TEntity>> GetAllByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default) =>
        _conn.QueryAsync<TEntity>(queryProvider.GetQuery<CommandDefinition>());

    public Task<TEntity?> GetByIdAsync(long id, CancellationToken token = default) =>
        _conn.GetAsync(new TEntity { Id = id });

    public Task<TEntity?> GetOneByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default) =>
        _conn.QuerySingleOrDefaultAsync<TEntity>(queryProvider.GetQuery<CommandDefinition>());

    public Task<bool> AnyByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default) =>
        _conn.QuerySingleAsync<bool>(queryProvider.GetQuery<CommandDefinition>());

    public Task<long> CountByQueryAsync(ICompiledQueriesProvider queryProvider, CancellationToken token = default) =>
        _conn.QuerySingleAsync<long>(queryProvider.GetQuery<CommandDefinition>());

    public async Task<TEntity> AddOrUpdateAsync(TEntity entity, long? id = null, bool saveChange = true, CancellationToken token = default)
    {
        var addUpdateEntity = id is not null ? entity with { Id = id.Value } : entity;

        if (id == null || id < 0)
        {
            await _conn.InsertAsync(addUpdateEntity);
        }
        else
        {
            await _conn.UpdateAsync(addUpdateEntity);
        }

        return addUpdateEntity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool saveChange = true, CancellationToken token = default)
    {
        await _conn.InsertAsync(entities);
        return entities;
    }

    public Task<bool> RemoveAsync(TEntity entity, bool saveChange = true, CancellationToken token = default) =>
        _conn.DeleteAsync(entity);
}