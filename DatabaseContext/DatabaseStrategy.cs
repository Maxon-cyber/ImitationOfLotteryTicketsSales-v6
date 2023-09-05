using DatabaseContext.QueryProcessing;
using DatabaseContext.QueryProcessing.Databases;
using DatabaseContext.QueryProcessing.Databases.GetQueryResult;

namespace DatabaseContext;

public class DatabaseStrategy
{
    public static Database SelectDatabase(CurrentDatabase databases)
        => databases switch
        {
            CurrentDatabase.MSSQLDatabase => MSSQLDatabase.GetInstance(),
            CurrentDatabase.MySQLDatabase => MySQLDatabase.GetInstance(),
        };
}