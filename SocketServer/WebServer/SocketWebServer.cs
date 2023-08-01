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
    private bool _isRunning;
    private List<string>? _responseFromDatabase;
    private readonly byte[] _receivedData = new byte[1024];
    private readonly StringBuilder _sentData = new StringBuilder();
    private readonly Socket _tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    internal SocketWebServer() { }

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
                    Socket? tcpClient = _tcpListener.Accept();

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

                            _responseFromDatabase = MSSQLDatabase
                                                            .GetInstance()
                                                            .ExecuteReaderAsync(_sentData.ToString())
                                                            ?.Result
                                                            ?.TextResult;

                            _sentData.Clear();

                            string res = "";
                            foreach (string response in _responseFromDatabase)
                                res += response + "\n";

                            Logger.LogInformation(
                                ServerResponse.Ok,
                                $"Ответ пользователю {ConnectingToTheServer.ClientAddress}: ",
                                res,
                                StringWritingParameters.NewLine
                                );

                            tcpClient.Send(Encoding.UTF8.GetBytes(res));

                            Stop(tcpClient);
                        });
                    }
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
        catch (ArgumentNullException ex) when (_responseFromDatabase is null)
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

            Logger.LogInformation(
                ServerResponse.ConnectionClosed,
                "Соединение закрыто",
                StringWritingParameters.NewLine
                );

            Logger.LogSeparator(
                '-', 
                110, 
                StringWritingParameters.NewLine
                );
        }
    }
}