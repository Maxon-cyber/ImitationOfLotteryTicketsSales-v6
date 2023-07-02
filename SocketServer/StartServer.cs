namespace SocketServer;

internal class StartServer
{
    private static void Main()
    {
        Console.Title = "Server";
        new WebServer.SocketWebServer().Start();
    }
}