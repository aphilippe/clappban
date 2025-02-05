using System.IO.Abstractions;
using Clappban.Models.Boards;
using Clappban.Navigation;
using Clappban.Navigation.Navigators;
using Clappban.Navigation.Navigators.specifics;
using Clappban.Navigation.Navigators.Specifics.Task;
using Clappban.Utils.IdGenerators;

namespace Clappban.ViewModels.Factories;

public interface ITaskViewModelFactory
{
    TaskViewModel Create(Task task, Board board);
}

public class TaskViewModelFactory : ITaskViewModelFactory
{
    private readonly IViewModelPresenter _viewModelPresenter;
    private readonly INavigator _finishEditingNavigator;

    public TaskViewModelFactory(IViewModelPresenter viewModelPresenter, INavigator finishEditingNavigator)
    {
        _viewModelPresenter = viewModelPresenter;
        _finishEditingNavigator = finishEditingNavigator;
    }

    public TaskViewModel Create(Task task, Board board)
    {
        var navigator = new EditTaskNavigator(_viewModelPresenter, _finishEditingNavigator,
            new FileSystem(), new TaskFilePathGenerator(board, new GuidGenerator()));
        
        return new TaskViewModel(task, board, navigator);
    }
}