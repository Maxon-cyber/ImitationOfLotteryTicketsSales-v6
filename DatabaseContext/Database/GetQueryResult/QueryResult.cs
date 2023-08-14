using Logging;
using Logging.StringRecordingParameters;
using System.Data.SqlClient;
using System.Collections.Concurrent;

namespace DatabaseContext.Database.GetQueryResult;

internal static class QueryResult
{
    internal static async Task<ConcurrentQueue<string>?> GetReaderResultAsync(SqlCommand command)
    {
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        ConcurrentQueue<string>? response = new ConcurrentQueue<string>();

        int countQueryResult = 0;

        try
        {
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    object? id = reader["Id"];

                    if (id is null)
                    {
                        await Logger.LogErrorAsync(
                            "Билет не найден",
                            StringWritingParameters.NewLine
                            );
                        return null;
                    }

                    countQueryResult++;

                    object? status = reader["Status"];
                    object? winningAmount = reader["WinningAmount"];
               
                    response.Enqueue(
                        $"\nTicketCount: {countQueryResult} " +
                        $"\n\tId: {id} " +
                        $"\n\tStatus: {status} " +
                        $"\n\tWinning Amount: {winningAmount}\n"
                        );
                }
            }
        }
        catch (SqlException ex) when (!reader.HasRows)
        {
            await Logger.LogErrorAsync(
                $"База Данных пуста \n{ex}",
                StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException ex) when (reader is null)
        {
            await Logger.LogErrorAsync(
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }

        return response;
    }

    internal static async Task<int> GetNonQueryResultAsync(SqlCommand command)
    {
        int result = await command.ExecuteNonQueryAsync();

        return result;
    }

    internal static async Task<object> GetScalarResultAsync(SqlCommand command)
    {
        object result = await command.ExecuteScalarAsync();

        return result;
    }
}