using System;
using System.Collections.Generic;
using Clappban.Kbn.Readers.LineReaders.Actions;
using Clappban.Kbn.Readers.LineReaders.Conditions;

namespace Clappban.Kbn.Readers.LineReaders;

public class ParameterizedLineReader : ILineReader
{
    private readonly ILineReaderCondition _condition;
    private readonly ILineReaderAction _action;

    public ParameterizedLineReader(ILineReaderCondition condition, ILineReaderAction action)
    {
        _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public bool IsValid(string line)
    {
        return _condition.Test(line);
    }

    public void Action(string line)
    {
        _action.Run(line);
    }

    public IEnumerable<ILineReader> NextPossibleReaders { get; set; }
}