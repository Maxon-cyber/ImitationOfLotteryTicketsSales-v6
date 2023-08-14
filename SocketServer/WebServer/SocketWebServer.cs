using ConnectionParameters;
using DatabaseContext.Database.Query;
using Logging;
using Logging.StringRecordingParameters;
using ResponseFromTheServer;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer.WebServer;

internal class SocketWebServer
{
    private const int COUNT_OF_REPEAT_SEPARATOR = 125;

    private List<string>? _responseFromDatabase;
    private readonly byte[] _receivedData = new byte[1024];
    private readonly StringBuilder _sentData = new StringBuilder();
    private readonly Socket _tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    internal SocketWebServer() { }

    internal void Start()
    {
        try
        {
            Logger.LogInformationAsync(
                ServerResponse.ServerSuccessfullyStarted,
                "\nСервер запущен\nОжидание подключений...",
                StringWritingParameters.NewLine
                );

            Logger.LogSeparatorAsync(
                '-',
                COUNT_OF_REPEAT_SEPARATOR,
                StringWritingParameters.NewLine
                );

            _tcpListener.Bind(
                new IPEndPoint(
                    ConnectingToTheServer.IP,
                    ConnectingToTheServer.Port
                    ));
            _tcpListener.Listen(ConnectingToTheServer.ListeningState);

            while (true)
            {
                Socket? tcpClient = _tcpListener.Accept();

                if (tcpClient.Connected)
                {
                    Logger.LogInformationAsync(
                        ServerResponse.ConnectionIsStable,
                        $"Пользователь {ConnectingToTheServer.ClientAddress} подключился",
                        StringWritingParameters.NewLine
                        );

                    Task.Run(() =>
                    {
                        do
                        {
                            int dataSize = tcpClient.Receive(_receivedData);

                            _sentData.Append(Encoding.UTF8.GetString(_receivedData, 0, dataSize));
                        }
                        while (tcpClient.Available > 0);

                        Logger.LogInformationAsync(
                              ServerResponse.RequestProcessedSuccessfully,
                              $"Пользователь {ConnectingToTheServer.ClientAddress} запросил: ",
                              _sentData.ToString(),
                              StringWritingParameters.NewLine
                              );

                        _responseFromDatabase = MSSQLDatabase
                                                        .GetInstance("MSSQLDatabase")
                                                        .ExecuteReaderAsync(_sentData.ToString())
                                                        ?.Result
                                                        ?.TextResult;

                        _sentData.Clear();

                        string? result = GetResult(_responseFromDatabase) ?? "null";

                        Logger.LogInformationAsync(
                            ServerResponse.Ok,
                            $"Ответ пользователю {ConnectingToTheServer.ClientAddress}: ",
                            result,
                            StringWritingParameters.NewLine
                            );

                        tcpClient.Send(Encoding.UTF8.GetBytes(result));

                        Stop(tcpClient);
                    });
                }
            }
        }
        catch (SocketException ex) when (!_tcpListener.Blocking)
        {
            Logger.LogErrorAsync(
                ServerResponse.ConnectionIsInterrupted,
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }
    }

    private string? GetResult(List<string> responseFromDatabase)
    {
        string? result = null;

        foreach (string? item in responseFromDatabase)
            result += item;

        return result;
    }

    private void Stop(Socket tcpClient)
    {
        tcpClient.Shutdown(SocketShutdown.Both);
        tcpClient.Close();

        Logger.LogInformationAsync(
            ServerResponse.ConnectionClosed,
            "Соединение закрыто",
            StringWritingParameters.NewLine
            );

        Logger.LogSeparatorAsync(
            '-',
            COUNT_OF_REPEAT_SEPARATOR,
            StringWritingParameters.NewLine
            );
    }
}