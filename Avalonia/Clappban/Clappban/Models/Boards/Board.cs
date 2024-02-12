using System.Collections.Generic;
using Avalonia.Platform.Storage;

namespace Clappban.Models.Boards;

public class Board
{
    public Board(string name, IEnumerable<Column> columns, IStorageFile file)
    {
        Name = name;
        Columns = columns;
        File = file;
    }

    public string Name { get; }
    public IEnumerable<Column> Columns { get; }
    public IStorageFile File { get; }
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

    public Task(string title)
    {
        Title = title;
    }
}