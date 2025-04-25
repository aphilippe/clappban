using System;
using Clappban.Models.Boards.Utils;
using NUnit.Framework;

namespace Clappban.Tests.Models.Boards.Utils;

[TestFixture]
[TestOf(typeof(TaskStringExtractor))]
public class TaskStringExtractorTests
{
    [Test]
    public void Test_Extract_WhenLineIsEmpty_ThrowException()
    {
        Assert.Throws<ArgumentException>(() => TaskStringExtractor.Extract("", ""));
    }

    [Test]
    public void Test_Extract_WhenTextOnlyHasTitle_ReturnDataWithTitle()
    {
        var title = "Task with only title";
        var result = TaskStringExtractor.Extract(title, "");
        Assert.That(result.Title, Is.EqualTo(title));
        Assert.That(result.FilePath, Is.Empty);
    }

    [Test]
    public void Test_Extract_WhenTextHasTitleAndFilePath_ReturnTaskWithTitleAndPath()
    {
        var title = "Task title";
        var path = "task/taskfile.kbn";

        var line = $"{title} | [{path}]";

        var result = TaskStringExtractor.Extract(line, @"c:\Board.kbn");
        
        Assert.That(result.Title, Is.EqualTo(title));
        Assert.That(result.FilePath, Is.EqualTo(@"c:\task/taskfile.kbn"));
    }

    [Test]
    public void Test_Extract_WhenTextHasTitleAndMetadataButNotFilePath_ReturnTaskTitleAndMetadata()
    {
        var title = "Task with only title | #tag";
        var result = TaskStringExtractor.Extract(title, "");
        Assert.That(result.Title, Is.EqualTo("Task with only title"));
        Assert.That(result.FilePath, Is.Empty);
        Assert.That(result.Metadata, Is.EqualTo("#tag"));
    }
    
    [Test]
    public void Test_Extract_WhenTextHasTitleAndMetadataAndFilePath_ReturnTaskTitleAndMetadataAndPath()
    {
        var title = "Task title";
        var path = "task/taskfile.kbn";
        var metadata = "#tag";

        var line = $"{title} | {metadata} [{path}]";

        var result = TaskStringExtractor.Extract(line, @"c:\Board.kbn");
        
        Assert.That(result.Title, Is.EqualTo(title));
        Assert.That(result.FilePath, Is.EqualTo(@"c:\task/taskfile.kbn"));
        Assert.That(result.Metadata, Is.EqualTo(metadata));
    }
    
    [Test]
    public void Test_Extract_WhenTextHasTitleAndMetadataAndFilePathButFilePathNotAtTheEnd_ReturnTaskWithTitleAndMetadata()
    {
        var title = "Task with only title | #tag [path/to/file] #tag2";
        var result = TaskStringExtractor.Extract(title, "");
        Assert.That(result.Title, Is.EqualTo("Task with only title"));
        Assert.That(result.FilePath, Is.Empty);
        Assert.That(result.Metadata, Is.EqualTo("#tag [path/to/file] #tag2"));
    }
}