namespace DatabaseContext.AddTicketToDatabase.CreatingTickets;

internal record class TicketEntity
{
    internal long Id { get; set; }
    internal string Status { get; set; }
    internal decimal WinningAmount { get; set; }
}