using System;
using System.Linq;
using System.Text;
using Clappban.Kbn.Serializers;

namespace Clappban.Models.Boards.Serialization;

public class ColumnKbnSectionInfoExtractor : IKbnSectionInfoExtractor<Column>
{
    private readonly ITaskSerializer _taskSerializer;

    public ColumnKbnSectionInfoExtractor(ITaskSerializer taskSerializer)
    {
        _taskSerializer = taskSerializer;
    }

    public string GetTitle(Column obj)
    { 
        return obj.Title;
    }

    public string GetContent(Column obj)
    {
        var allTaskString = obj.Tasks.Select(x => _taskSerializer.Serialize(x));
        return string.Join(Environment.NewLine, allTaskString);
    }
}