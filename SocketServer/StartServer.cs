namespace SocketServer;

internal class StartServer
{
    private static void Main()
    {
        Console.Title = "Server";
        Console.WindowWidth = 125;

        new WebServer.SocketWebServer().Start();
    }
}