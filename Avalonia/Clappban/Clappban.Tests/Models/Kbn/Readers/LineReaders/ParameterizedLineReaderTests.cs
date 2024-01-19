using System;
using Clappban.Kbn.Readers.LineReaders;
using Clappban.Kbn.Readers.LineReaders.Actions;
using Clappban.Kbn.Readers.LineReaders.Conditions;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Models.Kbn.Readers.LineReaders;

[TestFixture]
[TestOf(typeof(ParameterizedLineReader))]
public class ParameterizedLineReaderTests
{

    #region Constructor
    
    [Test]
    public void Test_Constructor_WhenConditionIsNull_ThrowArgumentNullException()
    {
        var action = Mock.Of<ILineReaderAction>();
        Assert.Throws<ArgumentNullException>(() => new ParameterizedLineReader(null, action));
    }
    
    [Test]
    public void Test_Constructor_WhenActionIsNull_ThrowArgumentNullException()
    {
        var condition = Mock.Of<ILineReaderCondition>();
        Assert.Throws<ArgumentNullException>(() => new ParameterizedLineReader(condition, null));
    }
    
    #endregion

    #region IsValid

    [Test]
    public void Test_IsValid_CallTestFromCondition()
    {
        var condition = Mock.Of<ILineReaderCondition>();
        var action = Mock.Of<ILineReaderAction>();

        var line = "line";
        
        var reader = new ParameterizedLineReader(condition, action);
        reader.IsValid(line);
        
        Mock.Get(condition).Verify(x => x.Test(line), Times.Once);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void Test_IsValid_ReturnValueOfConditionTest(bool testValue)
    {
        var condition = Mock.Of<ILineReaderCondition>();
        Mock.Get(condition).Setup(x => x.Test(It.IsAny<string>())).Returns(testValue);

        var action = Mock.Of<ILineReaderAction>();

        var reader = new ParameterizedLineReader(condition, action);
        var result = reader.IsValid("line");
        
        Assert.That(result, Is.EqualTo(testValue));
    }
    
    #endregion

    #region Action

    [Test]
    public void Test_Action_CallActionRun()
    {
        var condition = Mock.Of<ILineReaderCondition>();
        var action = Mock.Of<ILineReaderAction>();
        var line = "test";
        
        var reader = new ParameterizedLineReader(condition, action);
        reader.Action(line);
        
        Mock.Get(action).Verify(x => x.Run(line), Times.Once);
    }

    #endregion
}