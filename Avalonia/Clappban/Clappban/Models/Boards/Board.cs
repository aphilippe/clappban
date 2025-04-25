using System.Collections.Generic;
using Avalonia.Platform.Storage;

namespace Clappban.Models.Boards;

public class Board
{
    public Board(string name, IEnumerable<Column> columns, string filePath)
    {
        Name = name;
        Columns = columns;
        FilePath = filePath;
    }

    public string Name { get; }
    public IEnumerable<Column> Columns { get; }
    public string FilePath { get; }
}

public class Column
{
    public string Title { get; }
    public IEnumerable<Task> Tasks { get; }

    public Column(string title, IEnumerable<Task> tasks)
    {
        Title = title;
        Tasks = tasks;
    }
}

public class Task
{
    public string Title { get; }
    public string FilePath { get; set; }
    public string Metadata { get; set; }

    public Task(string title, string filePath, string metadata)
    {
        Title = title;
        FilePath = filePath;
        Metadata = metadata;
    }
}