using System.IO;
using Clappban.Models.Boards;
using Clappban.Models.Boards.Serialization;
using NUnit.Framework;

namespace Clappban.Tests.Models.Boards.Serialization;

[TestFixture]
[TestOf(typeof(TaskSerializer))]
public class TaskSerializerTests
{
    [Test]
    public void Test_Serialize_WhenOnlyTitle_ReturnTitle()
    {
        var task = new Task("title", string.Empty);
        
        var serializer = new TaskSerializer("");
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title"));
    }

    [Test]
    public void Test_Serialize_WhenPathIsNotEmpty_ReturnTitleWithRelativePath()
    {
        var path = @"C:\path\to\parent";
        var task = new Task("title", Path.Combine(path, "folder\\fileName.kbn"));
        
        var serializer = new TaskSerializer(path);
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title | [folder\\fileName.kbn]"));
    }

    // This test should be removed when tags will be managed (for now we use them directly in the title)
    [Test]
    public void Test_Serialize_WhenPathIsNotEmptyButThereIsTagInTitle_ReturnValidString()
    {
        var path = @"C:\path\to\parent";
        var task = new Task("title | #tag", Path.Combine(path, "folder\\fileName.kbn"));
        
        var serializer = new TaskSerializer(path);
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title | #tag [folder\\fileName.kbn]"));
    }
}