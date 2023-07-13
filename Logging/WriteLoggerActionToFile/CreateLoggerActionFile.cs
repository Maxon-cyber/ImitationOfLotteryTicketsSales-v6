namespace Logging.WriteLoggerActionToFile;

internal static class CreateLoggerActionFile
{
    private const string DIRECTION_NAME = "C://Пользователи/pc/ActionsInformation";

    private static void CreateFolder()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(DIRECTION_NAME);

        if (!dirInfo.Exists) dirInfo.Create();
    }

    internal static void WriteLoggerActionToFile(string loggerAction)
    {
        CreateFolder();

        File.WriteAllTextAsync(DIRECTION_NAME, loggerAction);
    }
}