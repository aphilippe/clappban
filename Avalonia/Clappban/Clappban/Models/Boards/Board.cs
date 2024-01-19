using System.Collections.Generic;

namespace Clappban.Models.Boards;

public class Board
{
    public Board(string name, IEnumerable<Column> columns)
    {
        Name = name;
        Columns = columns;
    }

    public string Name { get; }
    public IEnumerable<Column> Columns { get; }
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