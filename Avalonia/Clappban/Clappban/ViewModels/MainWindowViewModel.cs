using Clappban.InjectionDependency;
using Clappban.Navigation;
using ReactiveUI;
using Splat;

namespace Clappban.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase? ContentViewModel => _mainViewModelPresenter.CurrentViewModel;
    public ViewModelBase? ModalViewModel => _modalViewModelPresenter.CurrentViewModel;

    public string Title => _mainViewModelPresenter.Title;

    private readonly IViewModelPresenter _modalViewModelPresenter;
    private readonly MainViewModelPresenter _mainViewModelPresenter;
    
    public MainWindowViewModel(IViewModelPresenter modalViewModelPresenterPresenter,
        MainViewModelPresenter mainViewModelPresenter)
    {
        _modalViewModelPresenter = modalViewModelPresenterPresenter;
        _mainViewModelPresenter = mainViewModelPresenter;
        
        mainViewModelPresenter.Display(Locator.Current.GetRequiredService<OpenFileViewModel>());
        mainViewModelPresenter.CurrentViewModelChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(ContentViewModel));
            this.RaisePropertyChanged(nameof(Title));
        };

        _modalViewModelPresenter.CurrentViewModelChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(ModalViewModel));
        };
    }
}