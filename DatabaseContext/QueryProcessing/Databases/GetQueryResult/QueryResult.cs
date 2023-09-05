using Logging;
using Logging.StringRecordingParameters;
using System.Data.SqlClient;
using System.Collections.Concurrent;

namespace DatabaseContext.QueryProcessing.Databases.GetQueryResult;

internal sealed class QueryResult
{
    internal async Task<ConcurrentQueue<string>?> GetReaderResultAsync(SqlCommand command)
    {
        ConcurrentQueue<string>? response = new ConcurrentQueue<string>();

        await Task.Run(async () =>
        {
            await using SqlDataReader reader = await command.ExecuteReaderAsync();

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
                            await ConsoleLogger.LogErrorAsync(
                                "Билет не найден",
                                StringWritingParameters.NewLine
                                );
                            response = null;
                            break;
                        }

                        countQueryResult++;

                        object? status = reader["Status"];
                        object? winningAmount = reader["WinningAmount"];

                        response.Enqueue(
                            $"\nTicketCount: {countQueryResult}" +
                            $"\n\tId: {id}" +
                            $"\n\tStatus: {status}" +
                            $"\n\tWinning Amount: {winningAmount}\n"
                            );
                    }
                }
            }
            catch (SqlException ex) when (!reader.HasRows)
            {
                await ConsoleLogger.LogErrorAsync(
                    $"База Данных пуста \n{ex}",
                    StringWritingParameters.NewLine
                    );
            }
            catch (ArgumentNullException ex) when (reader is null)
            {
                await ConsoleLogger.LogErrorAsync(
                    ex.ToString(),
                    StringWritingParameters.NewLine
                    );
            }
        });

        return response;
    }

    internal async Task<int> GetNonQueryResultAsync(SqlCommand command)
    { 
        int result = await command.ExecuteNonQueryAsync();

        return result;
    }

    internal async Task<object> GetScalarResultAsync(SqlCommand command)
    { 
        object result = await command.ExecuteScalarAsync();

        return result;
    }
}