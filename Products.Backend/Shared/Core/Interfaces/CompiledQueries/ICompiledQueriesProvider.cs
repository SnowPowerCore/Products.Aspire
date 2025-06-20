namespace Products.Backend.Core.Interfaces.CompiledQueries;

public interface ICompiledQueriesProvider
{
    TCompiledQuery? GetQuery<TCompiledQuery>() where TCompiledQuery : notnull;
}