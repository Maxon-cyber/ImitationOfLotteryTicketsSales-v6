using DatabaseContext.Configuring.ConfiguringFilePath;
using DatabaseContext.Database.GetQueryResult;
using DatabaseContext.DeserializeData.DeserializeModels.ConnectionStringModel;
using Deserialize.YamlDeserialize;
using Logging;
using Logging.StringRecordingParameters;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseContext.Database.Query;

public sealed class MSSQLDatabase : IDatabase
{
    private readonly string? ConnectionToDatabase
        = @"Server=localhost\SQLEXPRESS;Database=LotteryTickets;Trusted_Connection=True;";

    public static MSSQLDatabase Singleton { get; } = new MSSQLDatabase();

    private MSSQLDatabase()
    {
        //ConnectionToDatabase = new DeserializerYaml<ConnectionStringModel>()
        //    ?.DeserializeConfiguringFile(ConfigFilePath.ConnectionString)
        //    ?.Result
        //    ?.ConnectionString
        //    ?.ConnectionString;

        Logger.LogInformation(
            "Запрос к базе данных",
            StringWritingParameters.NewLine
            );
    }

    public async Task<Result<List<string>>?> ExecuteReaderAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionToDatabase);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<List<string>>? result = new Result<List<string>>();

        try
        {
            await connection.OpenAsync();

            ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetReaderResultAsync(command)?.Result);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
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
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    public async Task<Result<object>?> ExecuteScalarAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionToDatabase);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<object>? result = new Result<object>();

        try
        {
            await connection.OpenAsync();

            ConnectingInformation(connection);

            result.TextResult =await Task.FromResult(QueryResult.GetScalarResultAsync(command)?.Result);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
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
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    public async Task<Result<int>> ExecuteNonQueryAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection();
        await using SqlCommand command = new SqlCommand();

        Result<int>? result = new Result<int>();

        try
        {
            await connection.OpenAsync();

            ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetNonQueryResultAsync(command).Result);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
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
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    private void ConnectingInformation(SqlConnection connection)
    {
        Logger.LogInformation(
              "Подключение открыто\n" +
              "Свойства подключения:\n" +
              $"\tСтрока подключения: {connection.ConnectionString}\n" +
              $"\tId подключения: {connection.ClientConnectionId}\n" +
              $"\tБаза данных: {connection.Database}\n" +
              $"\tСервер: {connection.DataSource}\n" +
              $"\tВерсия сервера: {connection.ServerVersion}\n" +
              $"\tСостояние: {connection.State}\n",
              StringWritingParameters.NewLine
              );
    }

    private async Task CloseConnectionAsync(SqlConnection connection)
    {
        if (connection.State == ConnectionState.Open)
        {
            await connection.CloseAsync();

            Logger.LogInformation(
                "Подключение закрыто\n" +
                $"\tСостояние: {connection.State}",
                StringWritingParameters.NewLine
                );
        }
    }
}