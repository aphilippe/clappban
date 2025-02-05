using System.Windows.Input;
using Clappban.InjectionDependency;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using Clappban.ViewModels.Factories;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly Task _task;
    private readonly Board _board;

    public string Title => _task.Title;
    public ICommand OpenTaskCommand { get; }

    public TaskViewModel(Task task, Board board, INavigator<Task> editTaskNavigator)
    {
        _task = task;
        _board = board;
        OpenTaskCommand = ReactiveCommand.Create(() =>
        {
            editTaskNavigator.Navigate(_task);
        });
    }
}