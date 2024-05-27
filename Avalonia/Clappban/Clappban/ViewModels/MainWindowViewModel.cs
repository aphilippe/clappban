using System.IO;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Kbn.Readers;
using Clappban.Modal;
using Clappban.Models.Boards;
using Clappban.Navigation;
using ReactiveUI;
using Splat;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public ViewModelBase? ContentViewModel => _mainViewModelPresenter.CurrentViewModel;

    private ModalViewModel? _modalViewModel;
    private readonly IViewModelPresenter _mainViewModelPresenter;

    public ModalViewModel? ModalViewModel
    {
        get => _modalViewModel;
        private set => this.RaiseAndSetIfChanged(ref _modalViewModel, value);
    }
    
    public MainWindowViewModel(IBoardRepository boardRepository, ModalViewModel modalViewModel,
        IViewModelPresenter mainViewModelPresenter)
    {
        _boardRepository = boardRepository;
        _modalViewModel = modalViewModel;
        _mainViewModelPresenter = mainViewModelPresenter;
        
        mainViewModelPresenter.Display(Locator.Current.GetRequiredService<OpenFileViewModel>());
        mainViewModelPresenter.CurrentViewModelChanged += (_, _) =>
        {
            this.RaisePropertyChanged(nameof(ContentViewModel));
        };
    }

    
}