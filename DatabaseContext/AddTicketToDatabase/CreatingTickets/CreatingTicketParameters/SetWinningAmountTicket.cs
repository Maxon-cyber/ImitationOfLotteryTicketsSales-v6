namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal class SetWinningAmountTicket
{
    internal static decimal SetWinningAmount(int minAmount, int maxAmount)
        => new Random().Next(minAmount, maxAmount);
}