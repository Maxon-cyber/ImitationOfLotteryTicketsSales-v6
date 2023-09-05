using DatabaseContext.QueyProcessing;
using Logging.StringRecordingParameters;
using Logging;
using System.Collections.Concurrent;

namespace DatabaseContext.QueryProcessing.Databases.GetQueryResult;

internal sealed class MySQLDatabase : Database
{
    private static readonly Lazy<MySQLDatabase> _instance = new Lazy<MySQLDatabase>(() => new MySQLDatabase());

    internal static MySQLDatabase GetInstance() => _instance.Value;

    private MySQLDatabase() : base(databaseName: "MySQLDatabase") 
    {
        ConsoleLogger.LogInformationAsync(
         "Запрос к базе данных",
         StringWritingParameters.NewLine
         );
    }

    public override async Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request)
        => await base.ExecuteReaderAsync(request);
}