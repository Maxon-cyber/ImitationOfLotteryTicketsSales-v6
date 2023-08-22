namespace LotteryTicketWebAPI.RequestProcessing;

internal readonly struct SQLRequest
{
    internal static string GetAllTicketsQuery()
        => $"SELECT * FROM dbo.AllTickets;";

    internal static string GetTicketAnIdQuery(long? id)
        => $"SELECT * FROM dbo.AllTickets WHERE Id = {id};";

    internal static string BuyTicketAnIdQuery(long? id)
        => $"SELECT * FROM dbo.AllTickets WHERE Id = {id};";
}