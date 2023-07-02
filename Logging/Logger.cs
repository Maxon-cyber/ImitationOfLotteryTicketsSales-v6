using Logging.StringRecordingParameters;
using Logging.WriteLoggerActionToFile;
using ResponseFromTheServer;

namespace Logging;

public class Logger
{
    private static string _loggerAction;

    public static void LogInformation(string message, StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        _loggerAction = $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Write(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"{_loggerAction}\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

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
                Console.Write(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"{_loggerAction}\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

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
                Console.Write($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(clientRequest);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Response: {(int)response} - {response} {message} ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(clientRequest + "\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogError(string message, StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        _loggerAction = $"LogInformation: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} - {message}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Write(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"{_loggerAction}\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

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
                Console.Write(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"{_loggerAction}\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }

    public static void LogSeparator(
        string separator,
        StringWritingParameters stringWritingParameters)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;

        _loggerAction = $"{separator}";

        switch (stringWritingParameters)
        {
            case StringWritingParameters.None:
                Console.Write(_loggerAction);
                break;
            case StringWritingParameters.NewLine:
                Console.WriteLine($"{_loggerAction}\n");
                break;
        }

        new CreateLoggerActionFile().WriteLoggerActionToFile(_loggerAction);

        Console.ResetColor();
    }
}