namespace Clappban.Kbn.Serializers;

public interface IKbnSerializer<in T>
{
    string Serialize(T obj);
}
