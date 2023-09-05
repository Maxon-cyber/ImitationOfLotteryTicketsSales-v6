using Logging.StringRecordingParameters;
using ResponseFromTheServer;

namespace Logging;

public static class ConsoleLogger
{
    public static async Task LogInformationAsync(
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}");
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}\n");
                break;
        }

        Console.ResetColor();
    }

    public static async Task LogInformationAsync(
        ServerResponse response,
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}");
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteLineAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}\n");
                break;
        }

        Console.ResetColor();
    }

    public static async Task LogInformationAsync(
        ServerResponse response,
        string message,
        string clientRequest,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}");
                Console.ForegroundColor = ConsoleColor.Blue;
                await Console.Out.WriteAsync(clientRequest);
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteLineAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message} ");
                Console.ForegroundColor = ConsoleColor.Blue;
                await Console.Out.WriteLineAsync(clientRequest + "\n");
                break;
        }

        Console.ResetColor();
    }

    public static async Task LogErrorAsync(
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}");
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteLineAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}\n");
                break;
        }


        Console.ResetColor();
    }

    public static async Task LogErrorAsync(
        ServerResponse error,
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync($"LogError: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)error} - {error} {message}");
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteLineAsync($"LogError: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)error} - {error} {message}\n");
                break;
        }

        Console.ResetColor();
    }

    public static async Task LogSeparatorAsync(
        char separator,
        int countRepeat,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                await Console.Out.WriteAsync(new string(separator, countRepeat));
                break;
            case StringWritingParameters.NewLine:
                await Console.Out.WriteLineAsync($"{new string(separator, countRepeat)}\n");
                break;
        }

        Console.ResetColor();
    }
}