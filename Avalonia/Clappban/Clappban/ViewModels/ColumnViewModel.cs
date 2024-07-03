using System.Collections.Generic;
using System.Linq;
using Clappban.Models.Boards;
using Clappban.ViewModels.Factories;

namespace Clappban.ViewModels;

public class ColumnViewModel : ViewModelBase
{
    private readonly Column _column;

    public string Title => _column.Title;
    
    public IEnumerable<TaskViewModel> Tasks { get; }

    public ColumnViewModel(Column column, ITaskViewModelFactory taskViewModelFactory)
    {
        _column = column;
        Tasks = _column.Tasks.Select(taskViewModelFactory.Create);
    }
}