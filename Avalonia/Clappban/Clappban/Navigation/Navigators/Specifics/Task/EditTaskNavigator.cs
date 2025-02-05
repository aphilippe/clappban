using System.IO.Abstractions;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators.Specifics.Task;
using Clappban.ViewModels;

namespace Clappban.Navigation.Navigators.specifics;

public class EditTaskNavigator : INavigator<Task>
{
    private readonly IViewModelPresenter _presenter;
    private readonly INavigator _finishEditingNavigator;
    private readonly IFileSystem _fileSystem;
    private readonly ITaskFilePathGenerator _filePathGenerator;

    public EditTaskNavigator(IViewModelPresenter presenter, INavigator finishEditingNavigator, IFileSystem fileSystem,
        ITaskFilePathGenerator filePathGenerator)
    {
        _presenter = presenter;
        _finishEditingNavigator = finishEditingNavigator;
        _fileSystem = fileSystem;
        _filePathGenerator = filePathGenerator;
    }

    public void Navigate(Task task)
    {
        var text = string.IsNullOrEmpty(task.FilePath) ? string.Empty : _fileSystem.File.ReadAllText(task.FilePath);
        var filePath = string.IsNullOrEmpty(task.FilePath) ? _filePathGenerator.Generate(task) : task.FilePath; 
        _presenter.Display(new EditFileViewModel(text, filePath, _finishEditingNavigator));
    }
}