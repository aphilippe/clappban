using System.IO;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Kbn.Readers;
using Clappban.Models.Boards;
using ReactiveUI;
using Splat;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;
    private ViewModelBase? _contentViewModel;

    public ViewModelBase? ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    
    public MainWindowViewModel(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
        _boardRepository.CurrentBoardChanged += (sender, args) =>
        {
            if (_boardRepository.CurrentBoard == null) return;
            ContentViewModel = Locator.Current.GetRequiredService<BoardViewModel>();
        };
        
        ContentViewModel = Locator.Current.GetRequiredService<OpenFileViewModel>();
    }

    
}