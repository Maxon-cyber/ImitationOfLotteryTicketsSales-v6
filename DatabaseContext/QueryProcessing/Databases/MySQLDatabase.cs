namespace DatabaseContext.QueryProcessing.Databases.Databases;

internal sealed class MySQLDatabase : Database
{
    private static readonly Lazy<MySQLDatabase> _instance =
         new Lazy<MySQLDatabase>(() => new MySQLDatabase());

    internal static MySQLDatabase GetInstance() => _instance.Value;

    private MySQLDatabase() : base(databaseName: "MySQLDatabase") { }

    //Overriding Methods Database
}