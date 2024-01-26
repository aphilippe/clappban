using Clappban.Models.Boards;

namespace Clappban.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly Task _task;

    public string Title => _task.Title;

    public TaskViewModel(Task task)
    {
        _task = task;
    }
}