using System.Collections.Generic;
using System.Linq;
using Clappban.Models.Boards;

namespace Clappban.ViewModels;

public class ColumnViewModel : ViewModelBase
{
    private readonly Column _column;

    private string _title;
    public string Title => _column.Title;
    
    public IEnumerable<TaskViewModel> Tasks { get; }

    public ColumnViewModel(Column column)
    {
        _column = column;
        Tasks = _column.Tasks.Select(x => new TaskViewModel(x));
    }
}