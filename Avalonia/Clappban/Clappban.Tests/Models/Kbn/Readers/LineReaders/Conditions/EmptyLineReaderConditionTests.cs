using Clappban.Kbn.Readers.LineReaders.Conditions;
using NUnit.Framework;

namespace Clappban.Tests.Models.Kbn.Readers.LineReaders.Conditions;

[TestFixture]
[TestOf(typeof(EmptyLineReaderCondition))]
public class EmptyLineReaderConditionTests
{
    [Test]
    public void Test_Test_WhenLineIsEmpty_ReturnTrue()
    {
        var condition = new EmptyLineReaderCondition();
        var result = condition.Test("");

        Assert.That(result, Is.True);
    }

    [Test]
    public void Test_Test_WhenLineContainsCharacters_ReturnFalse()
    {
        var condition = new EmptyLineReaderCondition();
        var result = condition.Test("oisdjfj");

        Assert.That(result, Is.False);
    }

    [TestCase("    ")]
    [TestCase("\t")]
    public void Test_Test_WhenLineContainsOnlyBlanks_ReturnFalse(string testCase)
    {
        var condition = new EmptyLineReaderCondition();
        var result = condition.Test(testCase);

        Assert.That(result, Is.False);
    }
}