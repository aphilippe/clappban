using System.IO;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.Kbn.Readers;
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
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);

        var boardBuilder = new BoardKbnBuilder();
        
        KbnFileReader.Read(streamReader, boardBuilder);

        var board = boardBuilder.Build();
        _boardRepository.CurrentBoard = board;
    }

    public ICommand ReadFileCommand { get; }
}