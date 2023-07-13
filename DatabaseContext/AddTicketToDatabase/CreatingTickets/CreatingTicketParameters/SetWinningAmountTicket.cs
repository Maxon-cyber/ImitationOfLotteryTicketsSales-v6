namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal static class SetWinningAmountTicket
{
    internal static decimal SetWinningAmount(int minAmount, int maxAmount)
        => new Random().Next(minAmount, maxAmount);
}