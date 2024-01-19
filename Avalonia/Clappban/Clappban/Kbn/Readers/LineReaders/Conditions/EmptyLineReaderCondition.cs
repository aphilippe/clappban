namespace Clappban.Kbn.Readers.LineReaders.Conditions;

public class EmptyLineReaderCondition : ILineReaderCondition
{
    public bool Test(string line)
    {
        return line.Length == 0;
    }
}