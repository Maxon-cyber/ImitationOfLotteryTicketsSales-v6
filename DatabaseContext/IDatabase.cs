using DatabaseContext.Database.Query;

namespace DatabaseContext;

internal interface IDatabase
{
    internal Task<Result<int>> ExecuteNonQueryAsync(string request);

    internal Task<Result<object>?> ExecuteScalarAsync(string request);

    internal Task<Result<List<string>>?> ExecuteReaderAsync(string request);
}