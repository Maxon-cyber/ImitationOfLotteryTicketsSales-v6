using DatabaseContext.Databases.Query;

namespace DatabaseContext;

public static class DatabaseFacade
{
    //public static IDatabase SelectDatabase(CurrentDatabase databases)
    //    => databases switch
    //    {
    //        CurrentDatabase.MSSQLDatabase => MSSQLDatabase.GetInstance(),
    //        CurrentDatabase.MySQLDatabase => MySQLDatabase.GetInstance(),
    //    };

    public static DbContext SelectDatabase(CurrentDatabase databases)
        => databases switch
        {
            CurrentDatabase.MSSQLDatabase => MSSQLDatabase.GetInstance(),
        };
}