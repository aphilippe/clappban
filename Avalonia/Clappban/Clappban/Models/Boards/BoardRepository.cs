using System;

namespace Clappban.Models.Boards;

public interface IBoardRepository
{
    Board? CurrentBoard { get; set; }

    event EventHandler CurrentBoardChanged;
}

public class BoardRepository : IBoardRepository
{
    private Board? _currentBoard;

    public Board? CurrentBoard
    {
        get => _currentBoard;
        set
        {
            _currentBoard = value;
            CurrentBoardChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? CurrentBoardChanged;
}