using System;
using System.Linq;
using Clappban.Models.Boards;
using Clappban.Models.Boards.Serialization;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Models.Boards.Serialization;

[TestFixture]
[TestOf(typeof(ColumnKbnSectionInfoExtractor))]
public class ColumnKbnSectionInfoExtractorTests
{
    [Test]
    public void Test_GetContent_WhenThereIsNoTask_ReturnEmptyString()
    {
        var column = new Column("title", Enumerable.Empty<Task>());
        var taskSerializer = Mock.Of<ITaskSerializer>();
        
        var extractor = new ColumnKbnSectionInfoExtractor(column, taskSerializer);
        var result = extractor.GetContent();
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Test_GetContent_WhenThereIsOneTask_ReturnOneTaskString()
    {
        var task = new Task("title", string.Empty, string.Empty);
        var column = new Column("title", new[] { task });

        var taskSerializer = Mock.Of<ITaskSerializer>();
        Mock.Get(taskSerializer).Setup(x => x.Serialize(It.IsAny<Task>())).Returns("Task serialized");
        
        var extractor = new ColumnKbnSectionInfoExtractor(column, taskSerializer);
        var result = extractor.GetContent();

        Assert.That(result, Is.EqualTo("Task serialized"));
    }

    [Test]
    public void Test_GetContent_WhenThereIsSeveralTasks_ReturnAllTasksString()
    {
        var tasks = Enumerable.Range(0, 10).Select(i => new Task(string.Empty, string.Empty, string.Empty));
        var column = new Column("title", tasks);
        var call = 0;
        
        var taskSerializer = Mock.Of<ITaskSerializer>();
        Mock.Get(taskSerializer).Setup(x => x.Serialize(It.IsAny<Task>())).Returns($"Task {call}");
        
        var extractor = new ColumnKbnSectionInfoExtractor(column, taskSerializer);
        var result = extractor.GetContent();
        
        var expectedResult = string.Join(Environment.NewLine, Enumerable.Range(0, 10).Select(i => $"Task {call}"));
        Assert.That(result, Is.EqualTo(expectedResult));
    }
}