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
    private string _request;
    private string _description;
    private readonly byte[] _buffer;
    private readonly StringBuilder _receivedData;
    private readonly Socket _tcpClient;

    internal static string? Response { get; private set; }

    internal ProcessingClientRequests(string httpMethod, int? id)
    {
        _buffer = new byte[512];
        _receivedData = new StringBuilder();
        _tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
        Logger.LogInformation(
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
        catch (SocketException ex) when (!_tcpClient.Connected)
        {
            Logger.LogError(
                ServerResponse.ConnectionIsInterrupted,
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }
        catch (Exception ex) when (ex is ArgumentException or ArgumentNullException)
        {
            Logger.LogError(
                ServerResponse.InvalidValue,
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }
    }

    private async Task ResponseReceivedFromServerAsync()
    {
        try
        {
            do
            {
                 int dataSize = await _tcpClient.ReceiveAsync(_buffer);

                _receivedData.Append(Encoding.UTF8.GetString(_buffer, 0, Convert.ToInt32(dataSize)));
            }
            while (_tcpClient.Available > 0);

            Logger.LogInformation(
                ServerResponse.Ok,
                $"Ответ пользователю {ConnectingToTheServer.ClientAddress} - ",
                _receivedData.ToString(),
                StringWritingParameters.NewLine
                );

            Response = _receivedData.ToString();

            Logger.LogSeparator(
                '-',
                110,
                StringWritingParameters.NewLine
                );
        }
        catch (ArgumentNullException ex) when (_receivedData is null)
        {
            Logger.LogError(
                ServerResponse.NotFound,
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }
    }

    private void EndSession()
    {
        //_request = null;
        //_description = null;
        _receivedData.Clear();
        _tcpClient.Shutdown(SocketShutdown.Both);
        _tcpClient.Close();
    }
}