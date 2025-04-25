using System.Globalization;
using System.IO;
using System.Text;

namespace Clappban.Models.Boards.Serialization;

public class TaskSerializer : ITaskSerializer
{
    private readonly string _path;

    public TaskSerializer(string path)
    {
        _path = path;
    }

    public string Serialize(Task task)
    {
        var stringBuilder = new StringBuilder(task.Title);
        
        if (!IsTaskContainMetadata(task)) return stringBuilder.ToString();

        stringBuilder.Append(" |");

        if (!string.IsNullOrEmpty(task.Metadata))
        {
            stringBuilder.Append(" " + task.Metadata);
        }

        if (!string.IsNullOrEmpty(task.FilePath))
        {
            var relativePath = Path.GetRelativePath(_path, task.FilePath);
            stringBuilder.Append($" [{relativePath}]");
        }

        return stringBuilder.ToString();
    }

    private static bool IsTaskContainMetadata(Task task)
    {
        return !string.IsNullOrEmpty(task.FilePath) || !string.IsNullOrEmpty(task.Metadata);
    } 
}