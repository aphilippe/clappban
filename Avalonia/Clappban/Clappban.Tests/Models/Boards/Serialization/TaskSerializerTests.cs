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
        var task = new Task("title", string.Empty, string.Empty);
        
        var serializer = new TaskSerializer("");
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title"));
    }

    [Test]
    public void Test_Serialize_WhenPathIsNotEmpty_ReturnTitleWithRelativePath()
    {
        var path = @"C:\path\to\parent";
        var task = new Task("title", Path.Combine(path, "folder\\fileName.kbn"), string.Empty);
        
        var serializer = new TaskSerializer(path);
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title | [folder\\fileName.kbn]"));
    }

    
    [Test]
    public void Test_Serialize_WhenPathIsNotEmptyAndMetadataNotEmpty_ReturnValidString()
    {
        var path = @"C:\path\to\parent";
        var task = new Task("title", Path.Combine(path, "folder\\fileName.kbn"), "#tag");
        
        var serializer = new TaskSerializer(path);
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title | #tag [folder\\fileName.kbn]"));
    }
    
    [Test]
    public void Test_Serialize_WhenPathIsEmptyAndMetadataNotEmpty_ReturnValidString()
    {
        var path = @"C:\path\to\parent";
        var task = new Task("title", string.Empty, "#tag");
        
        var serializer = new TaskSerializer(path);
        var result = serializer.Serialize(task);
        
        Assert.That(result, Is.EqualTo("title | #tag"));
    }
}