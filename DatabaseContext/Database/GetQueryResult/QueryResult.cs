using Logging;
using Logging.StringRecordingParameters;
using System.Data.SqlClient;

namespace DatabaseContext.Database.GetQueryResult;

internal static class QueryResult
{
    internal static async Task<List<string>?> GetReaderResultAsync(SqlCommand command)
    {
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        List<string> response = new List<string>();

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

                    //Logger.LogInformation(
                    //    $"Ответ из БД: {result.TextResult}",
                    //    StringWritingParameters.NewLine
                    //    );
                }
            }
        }
        catch (SqlException ex) when (!reader.HasRows)
        {
            //result.TextError = ex.ToString();

            Logger.LogError(
                $"База Данных пуста \n{ex}",
            StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException ex) when (reader is null)
        {
            //result.TextError = ex.ToString();

            Logger.LogError(
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }

        return response;
    }

    internal static async Task<Result<int>> GetNonQueryResultAsync(SqlCommand command)
    {
        Result<int>? result = new Result<int>();

        try
        {
            result.TextResult = await command.ExecuteNonQueryAsync();
        }
        catch (ArgumentNullException ex) when (result.TextResult is 0)
        {
            Logger.LogError(
              $"Столбцы для запроса {command.CommandText} не найдены \n{ex}",
              StringWritingParameters.NewLine
              );
        }
        catch (ArgumentNullException ex) when (command is null)
        {

        }

        return result;
    }

    internal static async Task<Result<object>?> GetScalarResultAsync(SqlCommand command)
    {
        Result<object>? result = new Result<object>();

        result.TextResult = await command.ExecuteScalarAsync();

        return result;
    }
}