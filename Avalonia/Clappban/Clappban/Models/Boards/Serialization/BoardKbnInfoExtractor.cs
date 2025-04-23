using System.Collections.Generic;
using Clappban.Kbn.Serializers;

namespace Clappban.Models.Boards.Serialization;

public class BoardKbnInfoExtractor : IKbnInfoExtractor<Board>
{
    public string GetTitle(Board obj)
    {
        return obj.Name;
    }

    public IEnumerable<object> GetSections(Board obj)
    {
        return obj.Columns;
    }
}