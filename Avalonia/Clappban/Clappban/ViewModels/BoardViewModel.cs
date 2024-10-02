using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Clappban.Models.Boards;
using Clappban.Navigation.Navigators;
using Clappban.ViewModels.Factories;
using ReactiveUI;

namespace Clappban.ViewModels;

public class BoardViewModel : ViewModelBase
{
    private readonly IColumnViewModelFactory _columnFactory;
    public Board Board { get; private set; }
    public IEnumerable<ColumnViewModel>? Columns { get; private set; }
    public ICommand ReloadCommand { get; }
    public ICommand EditCommand { get; }
    public override string Title => Board.Name;

    public BoardViewModel(Board board, IColumnViewModelFactory columnFactory, IBoardRepository boardRepository, INavigator<string> editBoardNavigator)
    {
        _columnFactory = columnFactory;
        Board = board;

        ReloadBoard();
        
        ReloadCommand =
            ReactiveCommand.CreateFromTask(async () =>
            {
                Board = await boardRepository.OpenAsync(Board.FilePath);
                ReloadBoard();
            });
        
        EditCommand = ReactiveCommand.Create(() =>
        {
            editBoardNavigator.Navigate(Board.FilePath);
        });
    }

    private void ReloadBoard()
    {
        Columns = Board.Columns.Select(_columnFactory.Create);
        this.RaisePropertyChanged(nameof(Columns));
    }
}