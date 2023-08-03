using Logging;
using Logging.StringRecordingParameters;
using System.Data.SqlClient;

namespace DatabaseContext.Database.GetQueryResult;

internal static class QueryResult
{
    internal static async Task<List<string>?> GetReaderResultAsync(SqlCommand command)
    {
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        List<string>? response = new List<string>();

        try
        {
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    object? id = reader["Id"];

                    if (id is null)
                    {
                        Logger.LogError(
                            "Билет не найден",
                            StringWritingParameters.NewLine
                            );
                        return null;
                    }

                    object? status = reader["Status"];
                    object? winningAmount = reader["WinningAmount"];
               
                    response.Add($"\nId: {id} \nStatus: {status} \nWinning Amount: {winningAmount}");
                }
            }
        }
        catch (SqlException ex) when (!reader.HasRows)
        {
            Logger.LogError(
                $"База Данных пуста \n{ex}",
            StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException ex) when (reader is null)
        {
            Logger.LogError(
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

    internal static async Task<object?> GetScalarResultAsync(SqlCommand command)
    {
        object? result = await command.ExecuteScalarAsync();

        return result;
    }
}