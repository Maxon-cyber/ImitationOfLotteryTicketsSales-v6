using ConnectionParameters;
using Logging;
using Logging.StringRecordingParameters;
using ResponseFromTheServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LotteryTicketWebAPI.RequestProcessing;

internal class ProcessingClientRequests
{
    private const int COUNT_OF_REPEAT_SEPARATOR = 130;

    private readonly string _request;
    private readonly string _description;
    private readonly byte[] _buffer = new byte[512];
    private readonly StringBuilder _receivedData = new StringBuilder();
    private readonly Socket _tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    internal static string? Response { get; private set; }

    internal ProcessingClientRequests(string httpMethod, int? id)
    {
        switch (httpMethod)
        {
            case "GetAllTickets":
                _request = SQLRequest.GetAllTicketsQuery();
                _description = "Получить все билеты";
                break;
            case "GetTicketAnId":
                _request = SQLRequest.GetTicketAnIdQuery(id);
                _description = "Получить билет по Id";
                break;
            case "BuyTicketAnId":
                _request = SQLRequest.BuyTicketAnIdQuery(id);
                _description = "Купить билет по Id";
                break;
        }
    }

    internal async Task SendDataClientToServerAsync()
    {
        await Logger.LogInformationAsync(
            ServerResponse.ConnectionIsStable,
            $"Пользователь {ConnectingToTheServer.ClientAddress} запросил - {_description}",
            StringWritingParameters.NewLine
            );

        try
        {
            await _tcpClient.ConnectAsync(
               new IPEndPoint(
                   ConnectingToTheServer.IP,
                   ConnectingToTheServer.Port
                   ));

            await _tcpClient.SendAsync(Encoding.UTF8.GetBytes(_request));

            await ResponseReceivedFromServerAsync();
            EndSession();
        }
        catch (SocketException ex) when (!_tcpClient.Blocking)
        {
            await Logger.LogErrorAsync(
                ServerResponse.ConnectionIsInterrupted,
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }
    }

    private async Task ResponseReceivedFromServerAsync()
    {
        do
        {
            int dataSize = await _tcpClient.ReceiveAsync(_buffer);

            _receivedData.Append(Encoding.UTF8.GetString(_buffer, 0, dataSize));
        }
        while (_tcpClient.Available > 0);

        await Logger.LogInformationAsync(
            ServerResponse.Ok,
            $"Ответ пользователю {ConnectingToTheServer.ClientAddress} - ",
            _receivedData.ToString(),
            StringWritingParameters.NewLine
            );

        Response = _receivedData.ToString();
    }

    private async Task EndSession()
    {
        _receivedData.Clear();
        _tcpClient.Shutdown(SocketShutdown.Both);
        _tcpClient.Close();

        await Logger.LogSeparatorAsync(
           '-',
           COUNT_OF_REPEAT_SEPARATOR,
           StringWritingParameters.NewLine
           );
    }
}