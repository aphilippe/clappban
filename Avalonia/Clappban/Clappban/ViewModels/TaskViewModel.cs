using System.Windows.Input;
using Clappban.InjectionDependency;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class TaskViewModel : ViewModelBase
{
    private readonly Task _task;

    public string Title => _task.Title;
    public ICommand OpenTaskCommand { get; }

    public TaskViewModel(Task task, INavigator<string> editTaskNavigator)
    {
        _task = task;
        OpenTaskCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // Locator.Current.GetRequiredService<ModalManager>().DisplayModal(new EditFileViewModel(_task.FilePath));
            // Locator.Current.GetRequiredService<NavigationService>().Display(typeof(EditFileViewModel), _task.FilePath);
            editTaskNavigator.Navigate(_task.FilePath);
        });
    }
}