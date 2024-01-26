using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    private readonly List<TaskBuilder> _tasks = new();

    public ColumnBuilder(string title)
    {
        _title = title;
    }

    public void AppendToContent(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        var pattern = @"^(?<title>.*)\s\[(?<file>[^\s\[\]]*)]$";
        var match = Regex.Match(text, pattern);

        if (!match.Success) throw new InvalidBoardFileException();
        
        _tasks.Add(new TaskBuilder(match.Groups["title"].Value, match.Groups["file"].Value));
    }

    public Column Build()
    {
        var tasks = _tasks.Select(x => x.Build());
        return new Column(_title, tasks);
    }
}

public class TaskBuilder
{
    private readonly string _title;
    private readonly string _filePath;

    public TaskBuilder(string title, string filePath)
    {
        _title = title;
        _filePath = filePath;
    }

    public Task Build()
    {
        // we do not use file for now
        return new Task(_title);
    }
}

public class InvalidBoardFileException : Exception
{
}