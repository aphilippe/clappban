using System.Collections;
using System.Collections.Generic;

namespace Clappban.Kbn.Serializers;

public interface IKbnInfoExtractor<in T>
{
    string GetTitle(T obj);
    IEnumerable GetSections(T obj);
}

public interface IKbnSectionInfoExtractor
{
    string GetTitle();
    string GetContent();
}