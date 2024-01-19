using System.Text.RegularExpressions;

namespace Clappban.Kbn.Readers.LineReaders.Conditions;

public class RegexLineReaderCondition : ILineReaderCondition
{
    private readonly Regex _regex;

    public RegexLineReaderCondition(string regexString)
    {
        _regex = new Regex(regexString);
    }

    public bool Test(string line)
    {
        return _regex.IsMatch(line);
    }
}