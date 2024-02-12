using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Clappban.Kbn.Readers;

namespace Clappban.Models.Boards;

public interface IBoardRepository
{
    Board? CurrentBoard { get; set; }

    event EventHandler CurrentBoardChanged;

    System.Threading.Tasks.Task OpenAsync(IStorageFile file);
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

    public async System.Threading.Tasks.Task OpenAsync(IStorageFile file)
    {
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);

        var boardBuilder = new BoardKbnBuilder(file);
        
        KbnFileReader.Read(streamReader, boardBuilder);

        var board = boardBuilder.Build();
        CurrentBoard = board;
    }
}