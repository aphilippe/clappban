using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using ReactiveUI;
using Splat;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class OpenFileViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;
    private readonly INavigator<Board> _boardOpenedNavigator;

    public OpenFileViewModel(IBoardRepository boardRepository, INavigator<Board> boardOpenedNavigator)
    {
        _boardRepository = boardRepository;
        _boardOpenedNavigator = boardOpenedNavigator;

        ReadFileCommand = ReactiveCommand.CreateFromTask<IStorageFile>(OpenFileAsync);
    }

    private async Task OpenFileAsync(IStorageFile file)
    {
        var board = await _boardRepository.OpenAsync(file.Path.LocalPath);
        
        if (board == null) return;
        
        // _navigationService.Display(typeof(BoardViewModel));
        _boardOpenedNavigator.Navigate(board);
    }

    public ICommand ReadFileCommand { get; }
}