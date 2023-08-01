using ResponseFromTheServer;

namespace LotteryTicketWebAPI.ResponseToTheClientFromTheServer;

internal static class ResponseOnSite
{
    internal static string Ok(string message) 
        => $"{(int)ServerResponse.Ok} - {ServerResponse.Ok}: \n{message}";

    internal static string ConnectionError(string message) 
        => $"{(int)ServerResponse.ConnectionIsInterrupted} - {ServerResponse.ConnectionIsInterrupted}: \n{message}";

    internal static string NotFound(string message) 
        => $"{(int)ServerResponse.NotFound} - {ServerResponse.NotFound}: \n{message}";

    internal static string BadRequest(string message)
        => $"{(int)ServerResponse.InvalidValue} - {ServerResponse.InvalidValue}: \n{message}";
}