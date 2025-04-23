namespace Clappban.Models.Boards.Serialization;

public interface ITaskSerializer
{
    public string Serialize(Task task);
}