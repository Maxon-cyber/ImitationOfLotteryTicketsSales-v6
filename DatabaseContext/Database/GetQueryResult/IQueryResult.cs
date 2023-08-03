using System.Data.SqlClient;
using DatabaseContext.Database.Query;

namespace DatabaseContext.Database.GetQueryResult;

internal interface IQueryResult
{
    Task<Result<string>?> GetReaderResultAsync(SqlCommand command);

    Task<Result<int>?> GetNonQueryResultAsync(SqlCommand command);

    Task<Result<object>?> GetScalarResultAsync(SqlCommand command);
}