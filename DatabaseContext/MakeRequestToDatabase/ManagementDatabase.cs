using Logging;
using Logging.StringRecordingParameters;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseContext.MakeRequestToDatabase;

public class ManagementDatabase
{
    private const string CONNECTION_STRING = @"Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
    private static string _response;

    public static async Task<string> MakeRequestToDbAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
        await using SqlCommand command = new SqlCommand(request, connection);

        try
        {
            await connection.OpenAsync();

            Logger.LogInformation(
                "Подключение открыто\n" +
                "Свойства подключения:\n" +
                $"\tСтрока подключения: {connection.ConnectionString}" +
                $"\tБаза данных: {connection.Database}" +
                $"\tСервер: {connection.DataSource}" +
                $"\tВерсия сервера: {connection.ServerVersion}" +
                $"\tСостояние: {connection.State}",
                StringWritingParameters.NewLine
                );

            await SendRequestToDbAsync(command);
        }
        catch (SqlException) when (connection.ConnectionTimeout > 15)
        {
            Logger.LogError(
                $"Время подключения к базе данных истекло - ",
                StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException) when (command.CommandText is null)
        {
            Logger.LogError(
               $"SQL запрос пуст",
               StringWritingParameters.NewLine
               );
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();

                Logger.LogError(
                    "Подключение закрыто" +
                    $"\tСостояние: {connection.State}",
                    StringWritingParameters.NewLine
                    );
            }
        }

        return _response;
    }

    private static async Task SendRequestToDbAsync(SqlCommand sqlCommand)
    {
        await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            while (await reader.ReadAsync())
            {
                object id = reader["Id"];
                object status = reader["Status"];
                object winningAmount = reader["WinningAmount"];

                _response = $"{id}\n{status}\n{winningAmount}";
            }
        }
        else
            _response = null;
    }
}