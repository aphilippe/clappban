using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.Models.Boards;
using ReactiveUI;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class OpenFileViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public OpenFileViewModel(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;

        ReadFileCommand = ReactiveCommand.CreateFromTask<IStorageFile>(OpenFileAsync);
    }

    private async Task OpenFileAsync(IStorageFile file)
    {
        await _boardRepository.OpenAsync(file);
    }

    public ICommand ReadFileCommand { get; }
}