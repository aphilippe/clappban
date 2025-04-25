using System;
using Avalonia.Controls.Shapes;
using Path = System.IO.Path;

namespace Clappban.Models.Boards.Utils;

public class TaskStringExtractor
{
    // TODO this code is UGLY
    
    private const string MetadataSeparator = " | ";
    
    public static Task Extract(string text, string boardPath)
    {
        if (string.IsNullOrEmpty(text)) throw new ArgumentException(nameof(text));

        var line = text.Trim();

        var contents = line.Split(MetadataSeparator);

        var path = string.Empty;
        var title = contents[0];
        var metadata = contents.Length > 1 ? contents[1] : string.Empty;
        if (metadata.EndsWith("]"))
        {
            var startPathIndex = metadata.LastIndexOf("[", StringComparison.Ordinal);
            var linepath = metadata.Substring(startPathIndex + 1, metadata.Length - startPathIndex - 2);
            path = Path.Combine(Path.GetDirectoryName(boardPath), linepath);
            metadata = metadata.Substring(0, startPathIndex).Trim();
        }
        
        return new Task(title, path, metadata);
    }
}