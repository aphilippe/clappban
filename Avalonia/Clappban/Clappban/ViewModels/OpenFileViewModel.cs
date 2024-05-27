using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Models.Boards;
using Clappban.Navigation;
using ReactiveUI;
using Splat;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class OpenFileViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;
    private readonly NavigationService _navigationService;

    public OpenFileViewModel(IBoardRepository boardRepository, NavigationService navigationService)
    {
        _boardRepository = boardRepository;
        _navigationService = navigationService;

        ReadFileCommand = ReactiveCommand.CreateFromTask<IStorageFile>(OpenFileAsync);
    }

    private async Task OpenFileAsync(IStorageFile file)
    {
        await _boardRepository.OpenAsync(file.Path.LocalPath);
        
        if (_boardRepository.CurrentBoard == null) return;
        
        _navigationService.Display(typeof(BoardViewModel));
    }

    public ICommand ReadFileCommand { get; }
}