namespace DatabaseContext.AddTicketToDatabase.CreatingTickets;

internal class TicketEntity
{
    internal long Id { get; set; }
    internal string Status { get; set; }
    internal DateTime CreationTime { get; set; }
    internal decimal WinningAmount { get; set; }
}