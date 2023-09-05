using DatabaseContext.Configuring.ConfiguringFilePath;
using DatabaseContext.DeserializeData.DeserializeModels.ConnectionStringModels;
using DatabaseContext.QueryProcessing.Databases.GetQueryResult;
using DatabaseContext.QueyProcessing;
using Deserialize;
using Logging;
using Logging.StringRecordingParameters;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseContext.QueryProcessing;

public abstract class Database : IDatabase
{
    private readonly string _connectionString;
    private readonly QueryResult _queryResult = new QueryResult();

    protected Database(string databaseName)
    {
        _connectionString = Deserializer<DatabaseModel>
            .Deserialize(ConfigFilePath.ConnectionString)
            .Database[databaseName]
            .ConnectionString;
    }

    public virtual async Task<Result<int>> ExecuteNonQueryAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand(request, connection)
        {
            CommandTimeout = 300
        };

        Result<int> result = new Result<int>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.Value = await Task.FromResult(_queryResult.GetNonQueryResultAsync(command).Result);

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

    public virtual async Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<ConcurrentQueue<string>>? result = new Result<ConcurrentQueue<string>>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.Value = await Task.FromResult(_queryResult.GetReaderResultAsync(command)?.Result);

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
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<object> result = new Result<object>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.Value = await Task.FromResult(_queryResult.GetScalarResultAsync(command)?.Result);

            await ConsoleLogger.LogInformationAsync(
                "Ответ из БД получен\n",
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

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;
    }

    ~Database() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}