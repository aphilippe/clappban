using System;
using System.IO;
using System.Threading.Tasks;
using Clappban.Kbn.Readers;

namespace Clappban.Models.Boards;

public interface IBoardRepository
{
    Task<Board?> OpenAsync(string filePath);
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

    public async Task<Board?> OpenAsync(string filePath)
    {
        using var streamReader = new StreamReader(filePath);
        
        var boardBuilder = new BoardKbnBuilder(filePath);
        
        KbnFileReader.Read(streamReader, boardBuilder);

        var board = boardBuilder.Build();
        return board;
    }
}