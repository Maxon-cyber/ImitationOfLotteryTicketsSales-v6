using Logging.StringRecordingParameters;
using Logging.WriteLoggerActionToFile;
using ResponseFromTheServer;

namespace Logging;

public static class Logger
{
    private static string _loggerAction;

    public static void LogInformation(string message, StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        _loggerAction = $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"{_loggerAction}\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogInformation(
        ServerResponse response,
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        _loggerAction = $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"{_loggerAction}\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogInformation(
        ServerResponse response,
        string message,
        string clientRequest,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        _loggerAction =
            $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message} {clientRequest}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Out.WriteAsync(clientRequest);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message} ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Out.WriteLineAsync(clientRequest + "\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogError(string message, StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        _loggerAction = $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"{_loggerAction}\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogError(
        ServerResponse error,
        string message,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        _loggerAction = $"LogError: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)error} - {error} {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"{_loggerAction}\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogSeparator(
        char separator,
        int countRepeat,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;

        _loggerAction = new string(separator, countRepeat);

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Out.WriteAsync(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.Out.WriteLineAsync($"{_loggerAction}\n");
                break;
        }

        CreateLoggerActionFile.WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }
}