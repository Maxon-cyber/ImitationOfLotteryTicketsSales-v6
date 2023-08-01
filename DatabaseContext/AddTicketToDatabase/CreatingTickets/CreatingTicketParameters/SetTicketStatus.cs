using DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters.TicketStatus;

namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal static class SetTicketStatus
{
    internal static string SetStatus() 
        => new Random().Next((int)Status.Sold, (int)Status.DoesNotExist) switch
        {
            1 => $"{Status.Sold}",
            2 => $"{Status.NotSold}",
            3 => $"{Status.Paid}",
            4 => $"{Status.DoesNotExist}"
        };
}