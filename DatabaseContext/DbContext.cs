using DatabaseContext.Configuring.ConfiguringFilePath;
using DatabaseContext.Database;
using DatabaseContext.Database.Databases.GetQueryResult;
using DatabaseContext.Databases.Query;
using DatabaseContext.DeserializeData.DeserializeModels.ConnectionStringModels;
using Deserialize.YamlDeserialize;
using Logging;
using Logging.StringRecordingParameters;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using YamlDotNet.Serialization.NamingConventions;

namespace DatabaseContext;

public abstract class DbContext : IDatabase
{
    internal readonly string ConnectionString;

    internal DbContext(string databaseName)
    {
        ConnectionString = new DeserializerYaml<DatabaseModel>()
            .DeserializeConfiguringFile(ConfigFilePath.ConnectionString, PascalCaseNamingConvention.Instance)
            .Database[databaseName]
            .ConnectionString;
    }

    public virtual async Task<Result<int>> ExecuteNonQueryAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionString);
        await using SqlCommand command = new SqlCommand(request);

        Result<int> result = new Result<int>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetNonQueryResultAsync(command).Result);

            await ConsoleLogger.LogInformationAsync("Ответ из БД получен\n",
               StringWritingParameters.NewLine
               );
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
        {
            await ConsoleLogger.LogErrorAsync(
                $"Время подключения к базе данных истекло \n{ex}",
                StringWritingParameters.NewLine
                );
        }
        finally
        {
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    public virtual async Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<ConcurrentQueue<string>>? result = new Result<ConcurrentQueue<string>>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetReaderResultAsync(command)?.Result);

            await ConsoleLogger.LogInformationAsync(
                "Ответ из БД получен\n",
                StringWritingParameters.NewLine
                );
        }
        catch (SqlException ex)
        {
            await ConsoleLogger.LogErrorAsync(
                $"Время подключения к базе данных истекло \n{ex}",
                StringWritingParameters.NewLine
                );
        }
        finally
        {
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    public virtual async Task<Result<object>> ExecuteScalarAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<object> result = new Result<object>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetScalarResultAsync(command)?.Result);

            await ConsoleLogger.LogInformationAsync("Ответ из БД получен\n",
               StringWritingParameters.NewLine
               );
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
        {
            await ConsoleLogger.LogErrorAsync(
                $"Время подключения к базе данных истекло \n{ex}",
                StringWritingParameters.NewLine
                );
        }
        finally
        {
            await CloseConnectionAsync(connection);
        }

        return result;
    }

    private async Task ConnectingInformation(SqlConnection connection)
    {
        await ConsoleLogger.LogInformationAsync(
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

            await ConsoleLogger.LogInformationAsync(
                "Подключение закрыто\n" +
                $"\tСостояние: {connection.State}\n",
                StringWritingParameters.NewLine
                );
        }
    }
}