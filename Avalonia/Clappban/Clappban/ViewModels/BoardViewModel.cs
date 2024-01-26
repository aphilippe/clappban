using System.Collections.Generic;
using System.Linq;
using Clappban.Models.Boards;

namespace Clappban.ViewModels;

public class BoardViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public Board Board => _boardRepository.CurrentBoard!;
    public IEnumerable<ColumnViewModel> Columns { get; }
    
    public BoardViewModel(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;

        Columns = _boardRepository.CurrentBoard.Columns.Select(x => new ColumnViewModel(x)).ToList();
    }
}