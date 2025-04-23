using System.IO.Abstractions;
using Clappban.Models.Boards;
using Clappban.Navigation;
using Clappban.Navigation.Navigators;
using Clappban.Utils.IdGenerators;
using Clappban.ViewModels.Factories.EditFileViewModel;
using TaskEditFileViewModelFactory = Clappban.ViewModels.Factories.EditFileViewModel.TaskEditFileViewModelFactory;

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
        var editFileViewModelFactory = new TaskEditFileViewModelFactory(_finishEditingNavigator,
            new FileSystem(), new TaskFilePathGenerator(board, new GuidGenerator()));
        var navigator = new ParameterNavigator<Task, ViewModels.EditFileViewModel>(_viewModelPresenter, t => editFileViewModelFactory.Create(t, board));
        
        return new TaskViewModel(task, board, navigator);
    }
}