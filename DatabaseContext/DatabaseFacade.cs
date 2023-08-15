using DatabaseContext.Database;
using DatabaseContext.Database.Query;

namespace DatabaseContext;

public static class DatabaseFacade
{
    public static IDatabase SelectDatabase(string databaseName)
        => databaseName.ToLower() switch
        {
            "mssqldatabase" => MSSQLDatabase.GetInstance(),
            "mysqldatabase" => null,
            "postgre" => null,
            "mongo" => null,
        };
}

public static class DatabaseFacade<TDatabase>
    where TDatabase: IDatabase, new()
{
    private class Nested
    {
        internal static readonly TDatabase Instance = new TDatabase();
    }

    public static TDatabase Instance => Nested.Instance;
}