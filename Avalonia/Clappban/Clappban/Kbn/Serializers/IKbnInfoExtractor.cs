using System.Collections.Generic;

namespace Clappban.Kbn.Serializers;

public interface IKbnInfoExtractor<in T>
{
    string GetTitle(T obj);
    IEnumerable<object> GetSections(T obj);
}

public interface IKbnSectionInfoExtractor<in T>
{
    string GetTitle(T obj);
    string GetContent(T obj);
}