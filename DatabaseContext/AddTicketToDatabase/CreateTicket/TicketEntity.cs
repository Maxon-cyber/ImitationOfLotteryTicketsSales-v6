namespace DatabaseContext.AddTicketToDatabase.CreateTicket;

internal class TicketEntity
{
    internal long Id { get; set; }
    internal string Status { get; set; }
    internal decimal WinningAmount { get; set; }
}