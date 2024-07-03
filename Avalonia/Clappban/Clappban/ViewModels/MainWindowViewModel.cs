using Clappban.InjectionDependency;
using Clappban.Navigation;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase? ContentViewModel => _mainViewModelPresenter.CurrentViewModel;
    public ViewModelBase? ModalViewModel => _modalViewModelPresenter.CurrentViewModel;

    private readonly IViewModelPresenter _modalViewModelPresenter;
    private readonly IViewModelPresenter _mainViewModelPresenter;
    
    public MainWindowViewModel(IViewModelPresenter modalViewModelPresenterPresenter,
        IViewModelPresenter mainViewModelPresenter)
    {
        _modalViewModelPresenter = modalViewModelPresenterPresenter;
        _mainViewModelPresenter = mainViewModelPresenter;
        
        mainViewModelPresenter.Display(Locator.Current.GetRequiredService<OpenFileViewModel>());
        mainViewModelPresenter.CurrentViewModelChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(ContentViewModel));
        };

        _modalViewModelPresenter.CurrentViewModelChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(ModalViewModel));
        };
    }
}