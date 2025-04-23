using System;
using System.Linq;
using System.Text;
using Clappban.Kbn.Serializers;

namespace Clappban.Models.Boards.Serialization;

public class ColumnKbnSectionInfoExtractor : IKbnSectionInfoExtractor
{
    private readonly ITaskSerializer _taskSerializer;
    private readonly Column _column;

    public ColumnKbnSectionInfoExtractor(Column column, ITaskSerializer taskSerializer)
    {
        _taskSerializer = taskSerializer;
        _column = column;
    }

    public string GetTitle()
    { 
        return _column.Title;
    }

    public string GetContent()
    {
        var allTaskString = _column.Tasks.Select(x => _taskSerializer.Serialize(x));
        return string.Join(Environment.NewLine, allTaskString);
    }
}