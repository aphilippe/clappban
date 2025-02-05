using System.Collections.Generic;
using System.Linq;
using Clappban.Models.Boards;
using Clappban.ViewModels.Factories;

namespace Clappban.ViewModels;

public class ColumnViewModel : ViewModelBase
{
    private readonly Column _column;
    private readonly Board _board;

    public string Title => _column.Title;
    
    public IEnumerable<TaskViewModel> Tasks { get; }

    public ColumnViewModel(Column column, ITaskViewModelFactory taskViewModelFactory, Board board)
    {
        _column = column;
        _board = board;
        Tasks = _column.Tasks.Select(x => taskViewModelFactory.Create(x, _board));
    }
}