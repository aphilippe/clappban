using System;
using System.Globalization;
using System.IO;
using Clappban.Models.Boards;
using Clappban.Utils.IdGenerators;

namespace Clappban.Navigation.Navigators.Specifics.Task;

public interface ITaskFilePathGenerator
{
    string Generate(Models.Boards.Task task);
}

public class TaskFilePathGenerator : ITaskFilePathGenerator
{
    private const string FolderAbsolutePath = "tasks";
    private const string FileExtension = "kbn";

    private readonly string _boardPath;
    private readonly IIdGenerator _idGenerator;

    public TaskFilePathGenerator(Board board, IIdGenerator idGenerator)
    {
        _boardPath = Path.GetDirectoryName(board.FilePath);
        _idGenerator = idGenerator;
    }

    public string Generate(Models.Boards.Task task)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        return !string.IsNullOrEmpty(task.FilePath) ? task.FilePath : GeneratePath(task.Title);
    }

    private string GeneratePath(string taskTitle)
    {
        var validTaskTitle = taskTitle.Replace(" ", "-");
        var id = _idGenerator.Generate();
        var fileNameWithExtension = $"{validTaskTitle}_{id}.{FileExtension}";
        return Path.Join(_boardPath, FolderAbsolutePath, fileNameWithExtension);
    }
}