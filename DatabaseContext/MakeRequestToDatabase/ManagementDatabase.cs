using Logging;
using Logging.StringRecordingParameters;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseContext.MakeRequestToDatabase;

public static class ManagementDatabase
{
    private const string CONNECTION_STRING = @"Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";

    public static async Task<string> MakeRequestToDbAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
        await using SqlCommand command = new SqlCommand(request, connection);

        string response = null;

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

            response = await SendRequestToDbAsync(command);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 15)
        {
            Logger.LogError(
                $"Время подключения к базе данных истекло \n{ex}",
                StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException ex) when (command.CommandText is null)
        {
            Logger.LogError(
               $"SQL запрос пуст \n{ex}",
               StringWritingParameters.NewLine
               );
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();

                Logger.LogInformation(
                    "Подключение закрыто" +
                    $"\tСостояние: {connection.State}",
                    StringWritingParameters.NewLine
                    );
            }
        }

        return response;
    }

    private static async Task<string> SendRequestToDbAsync(SqlCommand sqlCommand)
    {
        await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
        string response = null;

        if (reader.HasRows)
        {
            while (await reader.ReadAsync())
            {
                object id = reader["Id"];
                object status = reader["Status"];
                object winningAmount = reader["WinningAmount"];

                response = $"{id}\n{status}\n{winningAmount}";
            }
        }
        else
            response = null;

        return response;
    }
}