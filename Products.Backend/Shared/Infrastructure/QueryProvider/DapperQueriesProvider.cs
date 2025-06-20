using Dapper;
using Products.Backend.Core.Interfaces.CompiledQueries;

namespace Products.Backend.Infrastructure.QueryProvider;

public class DapperQueriesProvider<TQuery> : ICompiledQueriesProvider
{
    private CommandDefinition _compiledQuery;

    public TCompiledQuery? GetQuery<TCompiledQuery>() where TCompiledQuery : notnull =>
        (TCompiledQuery?)Convert.ChangeType(_compiledQuery, typeof(TCompiledQuery?));

    public static DapperQueriesProvider<TQuery> Create(CommandDefinition queryInstance) =>
        new() { _compiledQuery = queryInstance };
}