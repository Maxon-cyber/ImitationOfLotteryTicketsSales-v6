﻿using ConnectionParameters;
using Logging;
using Logging.StringRecordingParameters;
using ResponseFromTheServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LotteryTicketWebAPI.RequestProcessing;

internal class ProcessingClientRequests
{
    private readonly byte[] _buffer;
    private readonly string _request;
    private readonly string _description;
    private readonly StringBuilder _receivedData;
    private readonly Socket _tcpListener;

    internal static string Response { get; private set; }

    internal ProcessingClientRequests(string httpMethod, int? id)
    {
        _buffer = new byte[512];
        _receivedData = new StringBuilder();
        _tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        switch (httpMethod)
        {
            case "Get":
                _request = SQLRequest.GetRequest(id);
                _description = "Получить билет по Id";
                break;
            case "Post":
                _request = SQLRequest.PostRequest(id);
                _description = "Получить билет по ID";
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
            await _tcpListener.ConnectAsync(
               new IPEndPoint(
                   ConnectingToTheServer.IP,
                   ConnectingToTheServer.Port
                   ));

            await _tcpListener.SendAsync(Encoding.UTF8.GetBytes(_request));

            await ResponseReceivedFromServerAsync();
            EndSession();
        }
        catch (SocketException ex) when (!_tcpListener.Connected)
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
                 int dataSize = await _tcpListener.ReceiveAsync(_buffer);

                _receivedData.Append(Encoding.UTF8.GetString(_buffer, 0, Convert.ToInt32(dataSize)));
            }
            while (_tcpListener.Available > 0);

            Logger.LogInformation(
                ServerResponse.Ok,
                $"Ответ пользователю {ConnectingToTheServer.ClientAddress} - ",
                _receivedData.ToString(),
                StringWritingParameters.NewLine
                );

            Response = _receivedData.ToString();

            Logger.LogSeparator(
                new string('-', 110),
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
        _receivedData.Clear();
        _tcpListener.Shutdown(SocketShutdown.Both);
        _tcpListener.Close();
    }
}