using System;
using System.Linq;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;

namespace Clappban.ViewModels.Factories;

public class BoardViewModelFactory
{
    private readonly IColumnViewModelFactory _columnFactory;
    private readonly INavigator<string> _editBoardNavigator;
    private readonly IBoardRepository _boardRepository;

    public BoardViewModelFactory(IColumnViewModelFactory columnFactory, INavigator<string> editBoardNavigator, IBoardRepository boardRepository)
    {
        _columnFactory = columnFactory;
        _editBoardNavigator = editBoardNavigator;
        _boardRepository = boardRepository;
    }

    public BoardViewModel Create(Board board)
    {
        return new BoardViewModel(board, _columnFactory, _boardRepository, _editBoardNavigator);
    }
}