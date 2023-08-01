using DatabaseContext.Database.GetQueryResult;

namespace DatabaseContext;

internal interface IDatabase : IAsyncDisposable
{
    internal Task<Result<int>?> ExecuteNonQueryAsync(string request);

    internal Task<Result<object>?> ExecuteScalarAsync(string request);

    internal Task<Result<List<string>>?> ExecuteReaderAsync(string request);
}