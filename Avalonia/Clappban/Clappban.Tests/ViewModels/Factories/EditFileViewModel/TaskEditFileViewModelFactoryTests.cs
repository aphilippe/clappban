using System.IO.Abstractions;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using Clappban.ViewModels.Factories.EditFileViewModel;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.ViewModels.Factories.EditFileViewModel;

[TestFixture]
[TestOf(typeof(TaskEditFileViewModelFactory))]
public class TaskEditFileViewModelFactoryTests
{
    [Test]
    public void Test_Navigate_WhenTaskHasFilePath_NavigateToEditFileViewModelWithContentOfFile()
    {
        var fileContent = "fileContent";
        
        var finishNavigator = Mock.Of<INavigator>();
        var fileSystem = Mock.Of<IFileSystem>();
        var filePathGenerator = Mock.Of<ITaskFilePathGenerator>();
        
        var task = new Task("title", "filePath");
        Mock.Get(fileSystem).Setup(x => x.File.ReadAllText(It.IsAny<string>())).Returns(fileContent);
        
        var navigator = new TaskEditFileViewModelFactory(finishNavigator, fileSystem, filePathGenerator);
        var viewModel = navigator.Create(task, null);
        
        Assert.That(viewModel, Is.TypeOf<Clappban.ViewModels.EditFileViewModel>());
        
        Assert.That(viewModel.Text, Is.EqualTo(fileContent));
        Assert.That(viewModel.FilePath, Is.EqualTo("filePath"));
    }

    [Test]
    public void Test_Navigate_WhenTaskHasNoFilePath_NavigateToEditFileViewModelWithGeneratedContent()
    {
        var filePath = "generatedFilePath";
        
        var finishNavigator = Mock.Of<INavigator>();
        var fileSystem = Mock.Of<IFileSystem>();
        var filePathGenerator = Mock.Of<ITaskFilePathGenerator>();
        
        var task = new Task("title", null);

        Mock.Get(filePathGenerator).Setup(x => x.Generate(It.IsAny<Task>())).Returns(filePath);
        
        var factory = new TaskEditFileViewModelFactory(finishNavigator, fileSystem, filePathGenerator);
        var viewModel = factory.Create(task, null);
        
        Assert.That(viewModel, Is.TypeOf<Clappban.ViewModels.EditFileViewModel>());
        
        Assert.That(viewModel.Text, Is.EqualTo(string.Empty));
        Assert.That(viewModel.FilePath, Is.EqualTo(filePath));
    }
}