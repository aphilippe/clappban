using System.IO;
using System.Windows.Input;
using Avalonia.Controls;
using Clappban.InjectionDependency;
using Clappban.Modal;
using Clappban.Models.Boards;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly Task _task;

    public string Title => _task.Title;
    public ICommand OpenTaskCommand { get; }

    public TaskViewModel(Task task)
    {
        _task = task;
        OpenTaskCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Locator.Current.GetRequiredService<ModalManager>().DisplayModal(new EditFileViewModel(_task.FilePath, null));
        });
    }
}