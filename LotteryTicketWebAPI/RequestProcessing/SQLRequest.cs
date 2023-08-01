namespace LotteryTicketWebAPI.RequestProcessing;

internal readonly struct SQLRequest
{
    internal static string GetRequest()
        => "SELECT * FROM dbo.AllTickets;";

    internal static string PostRequest(int? id)
        => $"SELECT * FROM dbo.AllTickets WHERE Id = {id};";
}