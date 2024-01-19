namespace Clappban.Kbn.Readers.LineReaders.Conditions;

public class AlwaysTrueLineReaderCondition : ILineReaderCondition
{
    public bool Test(string line)
    {
        return true;
    }
}