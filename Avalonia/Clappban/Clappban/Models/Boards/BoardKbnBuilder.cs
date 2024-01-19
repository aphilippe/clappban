using System.Collections.Generic;
using System.Linq;
using Clappban.Kbn.Builders;

namespace Clappban.Models.Boards;

public class BoardKbnBuilder : IKbnBuilder
{
    private string _title = "";
    private readonly List<ColumnBuilder> _columnBuilders = new();

    public void SetTitle(string title)
    {
        _title = title;
    }

    public void AddSection(string title)
    {
        var columnBuilder = new ColumnBuilder(title);
        _columnBuilders.Add(columnBuilder);
    }

    public IKbnSectionBuilder LastSection()
    {
        return _columnBuilders.Last();
    }

    public Board Build()
    {
        var columns = _columnBuilders.Select(x => x.Build());
        return new Board(_title, columns);
    }
}

public class ColumnBuilder : IKbnSectionBuilder
{
    private readonly string _title;
    private readonly List<string> _tasks = new();

    public ColumnBuilder(string title)
    {
        _title = title;
    }

    public void AppendToContent(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        _tasks.Add(text);
    }

    public Column Build()
    {
        var tasks = _tasks.Select(x => new Task(x));
        return new Column(_title, tasks);
    }
}