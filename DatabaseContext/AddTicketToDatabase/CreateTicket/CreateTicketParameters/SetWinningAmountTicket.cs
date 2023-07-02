namespace DatabaseContext.AddTicketToDatabase.CreateTicket.CreateTicketParameters;

internal class SetWinningAmountTicket
{
    internal static decimal SetWinningAmount(int minAmount, int maxAmount)
        => new Random().Next(minAmount, maxAmount);
}