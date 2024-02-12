using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Clappban.Models.Boards;
using ReactiveUI;

namespace Clappban.ViewModels;

public class BoardViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public Board Board => _boardRepository.CurrentBoard!;
    public IEnumerable<ColumnViewModel>? Columns { get; }
    public ICommand ReloadCommand { get; }
    
    public BoardViewModel(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;

        Columns = _boardRepository.CurrentBoard?.Columns.Select(x => new ColumnViewModel(x)).ToList();

        ReloadCommand =
            ReactiveCommand.CreateFromTask(() => _boardRepository.OpenAsync(Board.File));
    }
}