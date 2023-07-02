namespace LotteryTicketWebAPI.RequestProcessing;

internal readonly struct SQLRequest
{
    internal static string GetRequest(int? id)
        => "SELECT * FROM Tickets";

    internal static string PostRequest(int? id)
        => $"SELECT * FROM Tickets WHERE {id} == Id";
}