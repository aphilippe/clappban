using System.Diagnostics.Metrics;
using Clappban.Models.Boards;

namespace Clappban.ViewModels.Factories;

public interface IColumnViewModelFactory
{
    ColumnViewModel Create(Column column);
}

public class ColumnViewModelFactory : IColumnViewModelFactory
{
    private readonly ITaskViewModelFactory _taskViewModelFactory;

    public ColumnViewModelFactory(ITaskViewModelFactory taskViewModelFactory)
    {
        _taskViewModelFactory = taskViewModelFactory;
    }

    public ColumnViewModel Create(Column column)
    {
        return new ColumnViewModel(column, _taskViewModelFactory);
    }
}