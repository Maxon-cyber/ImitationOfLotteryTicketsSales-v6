using DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

namespace DatabaseContext.AddTicketToDatabase.CreatingTickets;

internal static class CreatingTicket
{
    internal static List<TicketEntity> CreateTicket(int countTickets)
    {
        return Enumerable
            .Range(0, countTickets)
            .Select(ticket => new TicketEntity
            {
                Id = SetTicketID.CreateID(),
                Status = SetTicketStatus.SetStatus(),
                //CreationTime = SetTimeOfTicketCreation.CreationTime(),
                WinningAmount = SetWinningAmountTicket.SetWinningAmount(1_000, 1_000_000)
            }).ToList();
    }
}