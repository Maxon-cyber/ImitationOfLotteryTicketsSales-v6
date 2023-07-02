namespace Logging.WriteLoggerActionToFile;

internal class CreateLoggerActionFile
{
    private const string DIRECTION_NAME = "C://Пользователи/pc/ActionsInformation";

    private void CreateFolder()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(DIRECTION_NAME);

        if (!dirInfo.Exists) dirInfo.Create();
    }

    internal void WriteLoggerActionToFile(string loggerAction)
    {
        CreateFolder();

        File.WriteAllTextAsync(DIRECTION_NAME, loggerAction);
    }
}