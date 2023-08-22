using DatabaseContext.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseContext.Databases.Query;

internal sealed class MySQLDatabase : IDatabase
{ 
    private class Nested
    {
        internal static MySQLDatabase Instance => new MySQLDatabase();
    }

    internal static MySQLDatabase GetInstance() => Nested.Instance;

    private MySQLDatabase() { }

    public Task<Result<int>> ExecuteNonQueryAsync(string request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request)
    {
        Result<ConcurrentQueue<string>>? result = new Result<ConcurrentQueue<string>>();
        ConcurrentQueue<string> concurrentQueue = new ConcurrentQueue<string>();
        concurrentQueue.Enqueue("Yes");
      
        result.TextResult = concurrentQueue;

        return result;
    }

    public Task<Result<object>> ExecuteScalarAsync(string request)
    {
        throw new NotImplementedException();
    }
}