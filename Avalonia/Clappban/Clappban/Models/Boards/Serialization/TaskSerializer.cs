using System.Security;
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
        
        if (string.IsNullOrEmpty(task.FilePath)) return stringBuilder.ToString();

        // This is needed until we fully manage tags in task
        if (task.Title.Contains(" | "))
        {
            stringBuilder.Append(' ');
        }
        else
        {
            stringBuilder.Append(" | ");
        }

        var relativePath = task.FilePath.Replace(_path, string.Empty);
        stringBuilder.Append($"[{relativePath}]");
        
        return stringBuilder.ToString();
    }
}