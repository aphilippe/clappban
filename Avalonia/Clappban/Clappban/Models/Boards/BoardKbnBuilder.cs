using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls.Platform;
using Avalonia.Platform.Storage;
using Clappban.Kbn.Builders;

namespace Clappban.Models.Boards;

public class BoardKbnBuilder : IKbnBuilder
{
    private string _title = "";
    private readonly List<ColumnBuilder> _columnBuilders = new();
    private readonly string _filePath;

    public BoardKbnBuilder(string filePath)
    {
        _filePath = filePath;
    }

    public void SetTitle(string title)
    {
        _title = title;
    }

    public void AddSection(string title)
    {
        var columnBuilder = new ColumnBuilder(title, _filePath);
        _columnBuilders.Add(columnBuilder);
    }

    public IKbnSectionBuilder LastSection()
    {
        return _columnBuilders.Last();
    }

    public Board Build()
    {
        var columns = _columnBuilders.Select(x => x.Build());
        return new Board(_title, columns, _filePath);
    }
}

public class ColumnBuilder : IKbnSectionBuilder
{
    private readonly string _title;
    private readonly List<TaskBuilder> _tasks = new();
    private readonly string _boardFilePath;

    public ColumnBuilder(string title, string boardFilePath)
    {
        _title = title;
        _boardFilePath = boardFilePath;
    }

    public void AppendToContent(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        var pattern = @"^(?<title>.*)\s\[(?<file>[^\s\[\]]*)]$";
        var match = Regex.Match(text, pattern);

        if (!match.Success) throw new InvalidBoardFileException();

        string taskFilePath = Path.Combine(Path.GetDirectoryName(_boardFilePath), match.Groups["file"].Value);
        
        _tasks.Add(new TaskBuilder(match.Groups["title"].Value, taskFilePath));
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
        return new Task(_title, _filePath);
    }
}

public class InvalidBoardFileException : Exception
{
}