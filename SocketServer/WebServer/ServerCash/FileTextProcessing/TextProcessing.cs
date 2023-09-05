using System.Collections.Concurrent;

namespace SocketServer.WebServer.ServerCash.FileTextProcessing;

internal class TextProcessing
{
    internal static string TextSeparator => new string('-', 40);
    private const string END_POINT_FILE = "End";

    internal ConcurrentQueue<string> AddKeyAndTextSeparator(string key, ConcurrentQueue<string> value)
    {
        ConcurrentQueue<string> text = new ConcurrentQueue<string>();

        text.Enqueue(key);

        foreach (string result in value)
            text.Enqueue(result);

        text.Enqueue($"{TextSeparator}\n{END_POINT_FILE}");

        return text;
    }

    internal List<string> GetFileText(string path)
    {
        List<string> textFile = new List<string>();

        for (string text = " "; text is not "\n\r\n\n"; text = File.ReadAllText(path))
            textFile.Add(text);

        return textFile;
    }
}