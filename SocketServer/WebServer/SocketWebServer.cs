using System.Net.Sockets;
using System.Net;
using System.Text;
using Logging.StringRecordingParameters;
using Logging;
using ResponseFromTheServer;
using ConnectionParameters;
using DatabaseContext.MakeRequestToDatabase;

namespace SocketServer.WebServer;

internal class SocketWebServer
{
    private bool _isRunning;
    private readonly byte[] _receivedData;
    private readonly StringBuilder _sentData;
    private readonly Socket _tcpListener;

    internal SocketWebServer()
    {
        _receivedData = new byte[512];
        _sentData = new StringBuilder();
        _tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    internal void Start()
    {
        try
        {
            if (!_isRunning)
            {
                _isRunning = true;

                Logger.LogInformation(
                    ServerResponse.ServerSuccessfullyStarted,
                    "\nСервер запущен\nОжидание подключений...",
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
                    Socket tcpClient = _tcpListener.Accept();

                    if (tcpClient.Connected)
                    {
                        Logger.LogInformation(
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

                            Logger.LogInformation(
                                  ServerResponse.RequestProcessedSuccessfully,
                                  $"Пользователь {ConnectingToTheServer.ClientAddress} запросил: ",
                                  _sentData.ToString(),
                                  StringWritingParameters.NewLine
                                  );

                            string responseFromDatabase = ManagementDatabase
                                                                    .MakeRequestToDbAsync(_sentData.ToString())
                                                                    .Result;

                            _sentData.Clear();

                            Logger.LogInformation(
                                ServerResponse.Ok,
                                $"Ответ пользователю {ConnectingToTheServer.ClientAddress}: ",
                                responseFromDatabase,
                                StringWritingParameters.NewLine
                                );

                            tcpClient.Send(Encoding.UTF8.GetBytes(responseFromDatabase));

                            Stop(tcpClient);

                            Logger.LogSeparator(
                            new string('-', 110),
                            StringWritingParameters.NewLine
                            );
                        });
                    }
                    else
                        Logger.LogInformation(
                            ServerResponse.ConnectionClosed,
                            "Пользователь завершил соединение",
                            StringWritingParameters.NewLine
                            );

                }
            }
        }
        catch (SocketException ex) when (!_tcpListener.Blocking)
        {
            Logger.LogError(
                ServerResponse.ConnectionIsInterrupted,
                ex.ToString(),
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

    private void Stop(Socket tcpClient)
    {
        if (_isRunning)
        {
            _isRunning = false;
            tcpClient.Shutdown(SocketShutdown.Both);
            tcpClient.Close();
        }
    }
}