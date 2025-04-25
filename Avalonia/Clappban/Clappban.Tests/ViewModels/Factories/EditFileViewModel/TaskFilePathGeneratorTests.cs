using System;
using System.IO;
using System.Linq;
using Clappban.Models.Boards;
using Clappban.Utils.IdGenerators;
using Clappban.ViewModels.Factories.EditFileViewModel;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.ViewModels.Factories.EditFileViewModel;

[TestFixture]
[TestOf(typeof(TaskFilePathGenerator))]
public class TaskFilePathGeneratorTests
{
    [Test]
    public void Test_Generate_WhenTaskHasAFilePath_ReturnTheTaskFilePath()
    {
        var board = new Board("name", Enumerable.Empty<Column>(), "");
        var task = new Task("title", "filePath", string.Empty);
        var idGenerator = Mock.Of<IIdGenerator>();

        var generator = new TaskFilePathGenerator(board, idGenerator);

        var result = generator.Generate(task);
        
        Assert.That(result, Is.EqualTo("filePath"));
    }

    [Test]
    public void Test_Generate_WhenTaskIsNull_ThrowArgumentNullException()
    {
        var board = new Board("name", Enumerable.Empty<Column>(), "");
        var idGenerator = Mock.Of<IIdGenerator>();
        
        var generator = new TaskFilePathGenerator(board, idGenerator);
        
        Assert.Throws<ArgumentNullException>(() => generator.Generate(null));
        
    }

    [TestCase("titlewithoutspace", "titlewithoutspace")]
    [TestCase("title with space", "title-with-space")]
    public void Test_Generate_WhenFilePathIsNull_ReturnGeneratedFilePath(string taskTitle, string titleInFileName)
    {
        var board = new Board("title", Enumerable.Empty<Column>(), @"C:\plop\board.kbn");
        var taskAbsolutePath = "tasks";
        var task = new Task(taskTitle, null, String.Empty);
        var idGenerator = Mock.Of<IIdGenerator>();

        Mock.Get(idGenerator).Setup(x => x.Generate()).Returns("unicId");
        
        var generator = new TaskFilePathGenerator(board, idGenerator);

        var result = generator.Generate(task);
        
        Assert.That(result, Is.EqualTo(Path.Join("C:\\plop", taskAbsolutePath, $"{titleInFileName}_unicId.kbn")));
    }
}