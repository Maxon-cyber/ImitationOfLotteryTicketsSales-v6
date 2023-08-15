using DatabaseContext.Database.Query;

namespace DatabaseContext.Database;

public interface IDatabase
{
    Task<Result<int>> ExecuteNonQueryAsync(string request);

    Task<Result<object>> ExecuteScalarAsync(string request);

    Task<Result<List<string>>?> ExecuteReaderAsync(string request);
}