using Clappban.Models.Boards;

namespace Clappban.ViewModels;

public class BoardViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public BoardViewModel(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }
}