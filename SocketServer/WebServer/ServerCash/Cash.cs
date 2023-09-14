using SocketServer.WebServer.ServerCash.FileTextProcessing;
using SocketServer.WebServer.ServerCash.StorageLocation;
using System.Collections.Concurrent;

namespace SocketServer.WebServer.ServerCash;

internal class Cash
{
    private const int MAX_CAPACITY_STORAGE = 1_000;

    private static readonly string[] _textFile = null!;
    private static readonly TextProcessing _textProcessing = new TextProcessing();

    internal static long FreeSpace => MAX_CAPACITY_STORAGE - _textFile.LongLength;

    static Cash() => _textFile = _textProcessing.GetFileText(PathStorageLocation.Path);

    internal static void Add(string key, ConcurrentQueue<string> value)
    {
        if (_textFile.Length < MAX_CAPACITY_STORAGE)
            File.AppendAllLines(PathStorageLocation.Path, _textProcessing.AddKeyAndValue(key, value));
    }

    internal static ConcurrentQueue<string> TakeValue(string key)
    {
        ConcurrentQueue<string> value = new ConcurrentQueue<string>();

        int initialIndexKey = _textProcessing.GetKeyLineNumberFromFile(_textFile, key);

        foreach (string text in _textFile)
            if (text == key)
                for (int index = initialIndexKey; _textFile[index] != TextProcessing.TextSeparator; index++)
                    value.Enqueue($"{_textFile[index]}\n");

        return value;
    }

    internal static bool Contains(string key)
    {
        for (int index = 0; index < _textFile.Length; index++)
            if (_textFile[index] == key)
                return true;

        return false;
    }
}