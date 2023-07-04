using DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

namespace DatabaseContext.AddTicketToDatabase.CreatingTickets;

internal class CreatingTicket
{
    internal List<TicketEntity> CreateTicket(int countTickets)
    {
        return Enumerable
            .Range(0, countTickets)
            .Select(ticket => new TicketEntity
            {
                Id = SetTicketID.CreateTicketID(),
                Status = SetTicketStatus.SetStatus(),
                WinningAmount = SetWinningAmountTicket.SetWinningAmount(0, 1_000_000)
            }).ToList();
    }
}