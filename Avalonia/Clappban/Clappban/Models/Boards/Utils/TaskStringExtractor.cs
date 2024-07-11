using System;
using Avalonia.Controls.Shapes;
using Path = System.IO.Path;

namespace Clappban.Models.Boards.Utils;

public class TaskStringExtractor
{
    // TODO this code is UGLY
    public static Task Extract(string text, string boardPath)
    {
        if (string.IsNullOrEmpty(text)) throw new ArgumentException(nameof(text));

        var line = text.Trim();

        var path = "";
        var title = "";
        if (line.EndsWith("]"))
        {
            var startPathIndex = line.LastIndexOf("[", StringComparison.Ordinal);
            var linepath = line.Substring(startPathIndex + 1, line.Length - startPathIndex - 2);
            path = Path.Combine(Path.GetDirectoryName(boardPath), linepath);
            title = line.Substring(0, startPathIndex).Trim();
        }
        else
        {
            title = line;
        }
        
        
        return new Task(title, path);
    }
}