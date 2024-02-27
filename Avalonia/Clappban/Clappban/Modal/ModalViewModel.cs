using Clappban.ViewModels;
using ReactiveUI;

namespace Clappban.Modal;

public class ModalViewModel : ViewModelBase
{
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }
}