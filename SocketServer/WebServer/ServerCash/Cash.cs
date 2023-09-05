using SocketServer.WebServer.ServerCash.FileTextProcessing;
using SocketServer.WebServer.ServerCash.StorageLocation;
using System.Collections.Concurrent;

namespace SocketServer.WebServer.ServerCash;

internal class Cash
{
    private const int MAX_CAPACITY_STORAGE = 10_000;
    private readonly TextProcessing _textProcessing = new TextProcessing();
    private readonly FileInfo _file = new FileInfo(PathStorageLocation.Path);

    internal void Add(string key, ConcurrentQueue<string> value)
    {
        if (_file.Length < MAX_CAPACITY_STORAGE)
            File.AppendAllLines(PathStorageLocation.Path, _textProcessing.AddKeyAndTextSeparator(key, value));
    }

    internal ConcurrentQueue<string> TakeValue(string key)
    {
        ConcurrentQueue<string> value = new ConcurrentQueue<string>();

        foreach (string text in _textProcessing.GetFileText(PathStorageLocation.Path))
            if(text == key)
                while (text != TextProcessing.TextSeparator)
                    value.Enqueue(text);

        return value;
    }

    internal bool Contains(string key)
    {
        List<string> text = _textProcessing.GetFileText(PathStorageLocation.Path);

        for (int index = 0; index < text.Count; index++)
            if (text[index] == key)
                return true;

        return false;
    }
}