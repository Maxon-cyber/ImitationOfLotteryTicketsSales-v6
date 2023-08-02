using Logging;
using Logging.StringRecordingParameters;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ConnectionParameters;

public static class ConnectingToTheServer
{
    public static IPAddress IP { get; } = GetIPAddress();
    public static int Port { get; } = 8888;
    public static string IPString { get; } = GetIPAddress().ToString();
    public static string ClientAddress { get; } = $"{IP}:{Port}";
    public static int ListeningState { get; } = 1000;

    private static IPAddress GetIPAddress()
    {
        IPAddress[] ipAddresses = Dns.GetHostEntry(Environment.MachineName).AddressList;

        int ipAddressItem = 0;

        try
        {
            while (ipAddresses[ipAddressItem].AddressFamily != AddressFamily.InterNetwork)
                ipAddressItem++;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Logger.LogError(
                ex.ToString(),
                StringWritingParameters.NewLine
                );
        }

        return ipAddresses[ipAddressItem];
    }

    private static int[] GetAllPorts()
        => IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Select(port => port.Port)
                .ToArray();

    private static int GetAvailablePort()
    {
        int availablePort = new Random().Next(IPEndPoint.MinPort, IPEndPoint.MaxPort);

        while (GetAllPorts().Contains(availablePort))
            availablePort++;

        return availablePort;
    }
}