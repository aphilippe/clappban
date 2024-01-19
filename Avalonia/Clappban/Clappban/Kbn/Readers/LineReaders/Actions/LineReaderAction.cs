using System;

namespace Clappban.Kbn.Readers.LineReaders.Actions;

public class LineReaderAction : ILineReaderAction
{
    private Action<string> _action;

    public LineReaderAction(Action<string> action)
    {
        _action = action;
    }

    public void Run(string line)
    {
        _action(line);
    }
}