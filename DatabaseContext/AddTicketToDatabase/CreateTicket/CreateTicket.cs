using DatabaseContext.AddTicketToDatabase.CreateTicket.CreateTicketParameters;

namespace DatabaseContext.AddTicketToDatabase.CreateTicket;

internal class CreateTicket
{
    internal List<TicketEntity> CreateTicketAsync(int countTickets)
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