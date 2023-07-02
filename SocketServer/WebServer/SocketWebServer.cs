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
    private int _dataSize;
    private bool _isRunning;
    private byte[] _receivedData;
    private StringBuilder _sentData;
    private Socket _tcpListener;

    internal SocketWebServer()
    {
        _dataSize = 0;
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
                                _dataSize = tcpClient.Receive(_receivedData);

                                _sentData.Append(Encoding.UTF8.GetString(_receivedData, 0, _dataSize));
                            }
                            while (tcpClient.Available > 0);

                            Logger.LogInformation(
                                  ServerResponse.RequestProcessedSuccessfully,
                                  $"Пользователь {ConnectingToTheServer.ClientAddress} запросил: ",
                                  _sentData.ToString(),
                                  StringWritingParameters.NewLine
                                  );

                            Task<string> responseFromDatabase = ManagementDatabase
                                                                    .MakeRequestToDbAsync(_sentData.ToString());

                            _sentData.Clear();

                            Logger.LogInformation(
                                ServerResponse.Ok,
                                $"Ответ пользователю {ConnectingToTheServer.ClientAddress}: ",
                                responseFromDatabase.Result,
                                StringWritingParameters.NewLine
                                );

                            tcpClient.Send(Encoding.UTF8.GetBytes(responseFromDatabase.Result));

                            Stop(tcpClient);

                            Logger.LogSeparator(
                            new string('-', 110),
                            StringWritingParameters.NewLine
                            );
                        });
                    }

                }
            }
        }
        catch (SocketException ex) when (!_tcpListener.Connected)
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