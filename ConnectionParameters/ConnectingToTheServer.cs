using Logging;
using Logging.StringRecordingParameters;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ConnectionParameters;

public static class ConnectingToTheServer
{
    public static IPAddress IP => GetIPAddress();
    public static int Port => 8_888;
    public static string ClientAddress => Dns.GetHostName();
    public static int ListeningState => 1_000;

    private static IPAddress GetIPAddress()
    {
        IPAddress[] ipAddresses = Dns.GetHostEntry(Environment.MachineName).AddressList;

        int ipAddressItem = 0;

        try
        {
            while (ipAddresses[ipAddressItem].AddressFamily != AddressFamily.InterNetwork)
                ipAddressItem++;
        }
        catch (IndexOutOfRangeException ex)
        {
            ConsoleLogger.LogErrorAsync(
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