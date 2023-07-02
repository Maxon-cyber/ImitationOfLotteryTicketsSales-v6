using System.Net;
using System.Net.NetworkInformation;

namespace ConnectionParameters;

public readonly struct ConnectingToTheServer
{
    public static IPAddress IP { get; } = GetIPAddress();
    public static int Port { get; } = 8888;//GetAvailablePort(8888);
    public static string IPString { get; } = GetIPAddress().ToString();
    public static string ClientAddress { get; } = $"{IP}:{Port}";
    public static int ListeningState { get; } = 1000;

    private static IPAddress GetIPAddress()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Environment.MachineName);

        return ipHostInfo.AddressList[1];
    }

    private static bool IsFreePort(int port)
    {
        IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] listeners = properties.GetActiveTcpListeners();
        int[] openPorts = listeners.Select(item => item.Port).ToArray<int>();
        return openPorts.All(openPort => openPort != port);
    }

    private static int GetAvailablePort(int availablePort)
    {
        availablePort = (availablePort > 0) ? availablePort : new Random().Next(1, 65535);

        while (!IsFreePort(availablePort)) availablePort += 1;

        return availablePort;
    }
}