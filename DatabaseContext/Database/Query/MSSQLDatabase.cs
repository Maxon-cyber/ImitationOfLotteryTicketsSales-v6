using DatabaseContext.Configuring.ConfiguringFilePath;
using DatabaseContext.Database.GetQueryResult;
using DatabaseContext.DeserializeData.DeserializeModels.ConnectionStringModels;
using Deserialize.YamlDeserialize;
using Logging;
using Logging.StringRecordingParameters;
using System.Data;
using System.Data.SqlClient;
using YamlDotNet.Serialization.NamingConventions;

namespace DatabaseContext.Database.Query;

public sealed class MSSQLDatabase : IDatabase
{
    private static string _dataBaseName;
    private readonly string ConnectionString;

    private class Nested
    {
        internal static MSSQLDatabase Instance { get; } = new MSSQLDatabase(_dataBaseName);
    }

    public static MSSQLDatabase GetInstance(string DatabaseName)
    {
        _dataBaseName = DatabaseName;
        return Nested.Instance;
    }

    private MSSQLDatabase(string DatabaseName)
    {
        ConnectionString = new DeserializerYaml<DatabaseModel>()
            .DeserializeConfiguringFile(ConfigFilePath.ConnectionString, PascalCaseNamingConvention.Instance)
            .Database[DatabaseName]
            .ConnectionString;

        Logger.LogInformationAsync(
            "Запрос к базе данных",
            StringWritingParameters.NewLine
            );
    }

    public async Task<Result<List<string>>?> ExecuteReaderAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<List<string>>? result = new Result<List<string>>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetReaderResultAsync(command)?.Result?.ToList());
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
        {
            await Logger.LogErrorAsync(
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

    public async Task<Result<object>> ExecuteScalarAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection(ConnectionString);
        await using SqlCommand command = new SqlCommand(request, connection);

        Result<object> result = new Result<object>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult =await Task.FromResult(QueryResult.GetScalarResultAsync(command)?.Result);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
        {
            await Logger.LogErrorAsync(
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

    public async Task<Result<int>> ExecuteNonQueryAsync(string request)
    {
        await using SqlConnection connection = new SqlConnection();
        await using SqlCommand command = new SqlCommand();

        Result<int> result = new Result<int>();

        try
        {
            await connection.OpenAsync();

            await ConnectingInformation(connection);

            result.TextResult = await Task.FromResult(QueryResult.GetNonQueryResultAsync(command).Result);
        }
        catch (SqlException ex) when (connection.ConnectionTimeout > 30)
        {
            await Logger.LogErrorAsync(
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
        await Logger.LogInformationAsync(
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

            await Logger.LogInformationAsync(
                "Подключение закрыто\n" +
                $"\tСостояние: {connection.State}",
                StringWritingParameters.NewLine
                );
        }
    }
}