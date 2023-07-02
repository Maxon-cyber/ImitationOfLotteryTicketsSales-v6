using DatabaseContext.AddTicketToDatabase;

namespace DatabaseContext;

internal class StartDatabase
{
    private static void Main()
    {
        Console.Title = "Database";

        new AddTicket().Add();
    }
}