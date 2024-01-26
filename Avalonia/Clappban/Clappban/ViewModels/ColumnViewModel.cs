using Clappban.Models.Boards;

namespace Clappban.ViewModels;

public class ColumnViewModel : ViewModelBase
{
    private readonly Column _column;

    private string _title;
    public string Title => _column.Title;

    public ColumnViewModel(Column column)
    {
        _column = column;
    }
}