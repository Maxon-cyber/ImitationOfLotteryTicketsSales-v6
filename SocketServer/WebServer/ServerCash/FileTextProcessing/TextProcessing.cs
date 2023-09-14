using System.Collections.Concurrent;

namespace SocketServer.WebServer.ServerCash.FileTextProcessing;

internal class TextProcessing
{
    internal static string TextSeparator => new string('-', 40);

    internal ConcurrentQueue<string> AddKeyAndValue(string key, ConcurrentQueue<string> value)
    {
        ConcurrentQueue<string> text = new ConcurrentQueue<string>();

        text.Enqueue(key);

        foreach (string result in value)
            text.Enqueue(result);

        text.Enqueue(TextSeparator);

        return text;
    }

    internal string[] GetFileText(string path)
    {
        string[] text = File.ReadAllLines(path);

        return text;
    }

    internal int GetKeyLineNumberFromFile(string[] textFile, string key)
    {
        int keyLineNumber = 0;

        for (int index = 0; index < textFile.Length; index++)
        {
            if (textFile[index] == key)
            {
                keyLineNumber = index;
                break;
            }

        }

        return keyLineNumber;
    }
}