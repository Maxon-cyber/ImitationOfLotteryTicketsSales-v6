namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal static class SetTimeOfTicketCreation
{
    internal static DateTime CreationTime()
        => DateTime.Now.ToLocalTime();
}