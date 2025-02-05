using System.IO.Abstractions;
using Clappban.Models.Boards;
using Clappban.Navigation;
using Clappban.Navigation.Navigators;
using Clappban.Navigation.Navigators.specifics;
using Clappban.Navigation.Navigators.Specifics.Task;
using Clappban.ViewModels;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Navigators.Specifics;

[TestFixture]
[TestOf(typeof(EditTaskNavigator))]
public class EditTaskNavigatorTests
{
    [Test]
    public void Test_Navigate_WhenTaskHasFilePath_NavigateToEditFileViewModelWithContentOfFile()
    {
        var fileContent = "fileContent";
        
        var presenter = Mock.Of<IViewModelPresenter>();
        var finishNavigator = Mock.Of<INavigator>();
        var fileSystem = Mock.Of<IFileSystem>();
        var filePathGenerator = Mock.Of<ITaskFilePathGenerator>();
        
        var task = new Task("title", "filePath");
        
        ViewModelBase viewModel = null;
        Mock.Get(presenter).Setup(x => x.Display(It.IsAny<ViewModelBase>()))
            .Callback<ViewModelBase>(vm => viewModel = vm);

        Mock.Get(fileSystem).Setup(x => x.File.ReadAllText(It.IsAny<string>())).Returns(fileContent);
        
        var navigator = new EditTaskNavigator(presenter, finishNavigator, fileSystem, filePathGenerator);
        navigator.Navigate(task);
        
        Mock.Get(presenter).Verify(x => x.Display(It.IsAny<EditFileViewModel>()));
        Assert.That(viewModel, Is.TypeOf<EditFileViewModel>());
        
        var editvm = viewModel as EditFileViewModel;
        Assert.That(editvm.Text, Is.EqualTo(fileContent));
        Assert.That(editvm.FilePath, Is.EqualTo("filePath"));
    }

    [Test]
    public void Test_Navigate_WhenTaskHasNoFilePath_NavigateToEditFileViewModelWithGeneratedContent()
    {
        var filePath = "generatedFilePath";
        
        var presenter = Mock.Of<IViewModelPresenter>();
        var finishNavigator = Mock.Of<INavigator>();
        var fileSystem = Mock.Of<IFileSystem>();
        var filePathGenerator = Mock.Of<ITaskFilePathGenerator>();
        
        var task = new Task("title", null);
        
        ViewModelBase viewModel = null;
        Mock.Get(presenter).Setup(x => x.Display(It.IsAny<ViewModelBase>()))
            .Callback<ViewModelBase>(vm => viewModel = vm);

        Mock.Get(filePathGenerator).Setup(x => x.Generate(It.IsAny<Task>())).Returns(filePath);
        
        var navigator = new EditTaskNavigator(presenter, finishNavigator, fileSystem, filePathGenerator);
        navigator.Navigate(task);
        
        Mock.Get(presenter).Verify(x => x.Display(It.IsAny<EditFileViewModel>()));
        Assert.That(viewModel, Is.TypeOf<EditFileViewModel>());
        
        var editvm = viewModel as EditFileViewModel;
        Assert.That(editvm.Text, Is.EqualTo(string.Empty));
        Assert.That(editvm.FilePath, Is.EqualTo(filePath));
    }
}