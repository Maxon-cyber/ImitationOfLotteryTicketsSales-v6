using DatabaseContext.QueryProcessing;
using DatabaseContext.QueryProcessing.Databases;
using DatabaseContext.QueryProcessing.Databases.Databases;

namespace DatabaseContext;

public static class DatabaseFactory
{
    public static Database SelectDatabase(CurrentDatabase databases)
        => databases switch
        {
            CurrentDatabase.MSSQLDatabase => MSSQLDatabase.GetInstance(),
            CurrentDatabase.MySQLDatabase => MySQLDatabase.GetInstance(),
        };
}