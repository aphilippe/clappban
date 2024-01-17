using System.Collections.Generic;

namespace Clappban.Kbn.Readers.LineReaders;

public interface ILineReader
{
    bool IsValid(string line);
    void Action(string line);
    IEnumerable<ILineReader> NextPossibleReaders { get; set; }
}