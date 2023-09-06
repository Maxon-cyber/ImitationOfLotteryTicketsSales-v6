using SocketServer.WebServer.ServerCash.FileTextProcessing;
using SocketServer.WebServer.ServerCash.StorageLocation;
using System.Collections.Concurrent;

namespace SocketServer.WebServer.ServerCash;

internal class Cash
{
    private const int MAX_CAPACITY_STORAGE = 1_000;

    private readonly string[] _textFile = null!;
    private readonly TextProcessing _textProcessing = new TextProcessing();
    private readonly FileInfo _file = new FileInfo(PathStorageLocation.Path);
    
    internal long FreeSpaceInCash => MAX_CAPACITY_STORAGE - _textFile.LongLength;

    internal Cash() => _textFile = _textProcessing.GetFileText(PathStorageLocation.Path);

    internal void Add(string key, ConcurrentQueue<string> value)
    {
        if (_file.Length < MAX_CAPACITY_STORAGE)
            File.AppendAllLines(PathStorageLocation.Path, _textProcessing.AddKeyAndValue(key, value));
    }

    internal ConcurrentQueue<string> TakeValue(string key)
    {
        ConcurrentQueue<string> value = new ConcurrentQueue<string>();

        int initialIndexKey = _textProcessing.GetKeyLineNumberFromFile(_textFile, key);

        foreach (string text in _textFile)
            if (text == key)
                for (int index = initialIndexKey; _textFile[index] != TextProcessing.TextSeparator; index++)
                {
                    if (_textFile[index] != key)
                    value.Enqueue($"{_textFile[index]}\n");
                }

        return value;
    }

    internal bool Contains(string key)
    {
        for (int index = 0; index < _textFile.Length; index++)
            if (_textFile[index] == key)
                return true;

        return false;
    }
}