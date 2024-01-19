namespace Clappban.Kbn.Readers.LineReaders.Conditions;

public interface ILineReaderCondition
{
    bool Test(string line);
}