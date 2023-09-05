using DatabaseContext.QueyProcessing;
using Logging;
using Logging.StringRecordingParameters;
using System.Collections.Concurrent;

namespace DatabaseContext.QueryProcessing.Databases;

internal sealed class MSSQLDatabase : Database
{
    private static readonly Lazy<MSSQLDatabase> _instance = new Lazy<MSSQLDatabase>(() => new MSSQLDatabase());

    internal static MSSQLDatabase GetInstance() => _instance.Value;

    private MSSQLDatabase() : base(databaseName: "MSSQLDatabase")
    {
        ConsoleLogger.LogInformationAsync(
          "Запрос к базе данных",
          StringWritingParameters.NewLine
          );
    }

    public override async Task<Result<ConcurrentQueue<string>>?> ExecuteReaderAsync(string request)
        => await base.ExecuteReaderAsync(request);

    public override async Task<Result<object>> ExecuteScalarAsync(string request)
        => await base.ExecuteScalarAsync(request);

    public override async Task<Result<int>> ExecuteNonQueryAsync(string request)
        => await base.ExecuteNonQueryAsync(request);
}