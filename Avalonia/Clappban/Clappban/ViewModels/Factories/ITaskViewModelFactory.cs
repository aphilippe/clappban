using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;

namespace Clappban.ViewModels.Factories;

public interface ITaskViewModelFactory
{
    TaskViewModel Create(Task task);
}

public class TaskViewModelFactory : ITaskViewModelFactory
{
    private readonly INavigator<Task> _editTaskNavigator;

    public TaskViewModelFactory(INavigator<Task> editTaskNavigator)
    {
        _editTaskNavigator = editTaskNavigator;
    }

    public TaskViewModel Create(Task task)
    {
        return new TaskViewModel(task, _editTaskNavigator);
    }
}