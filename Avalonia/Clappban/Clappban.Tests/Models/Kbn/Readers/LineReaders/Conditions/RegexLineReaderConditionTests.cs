using System.Text.RegularExpressions;
using Clappban.Kbn.Readers.LineReaders.Conditions;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Models.Kbn.Readers.LineReaders.Conditions;

[TestFixture]
[TestOf(typeof(RegexLineReaderCondition))]
public class RegexLineReaderConditionTests
{
    [Test]
    public void Test_Test_WhenLineMatchRegex_ReturnTrue()
    {
        var condition = new RegexLineReaderCondition(@"[a-z]+");

        var result = condition.Test("test");
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void Test_Test_WhenLineNotMatchRegex_ReturnFalse()
    {
        var condition = new RegexLineReaderCondition(@"\d+");

        var result = condition.Test("test");
        
        Assert.That(result, Is.False);
    }
}