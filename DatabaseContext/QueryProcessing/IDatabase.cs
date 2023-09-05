using System.Collections.Concurrent;

namespace DatabaseContext.QueyProcessing;

public interface IDatabase : IDisposable
{
    Task<Result<int>> ExecuteNonQueryAsync(string request);

    Task<Result<object>> ExecuteScalarAsync(string request);

    Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request);
}