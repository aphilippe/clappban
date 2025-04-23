using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Clappban.Kbn;

namespace Clappban.Models.Boards.Utils;

public class BoardToKbnFactory
{
    public static Kbn.Kbn CreateKbn(Board board)
    {
        var sections = board.Columns.Select(CreateSection);
        return new Kbn.Kbn(board.Name, sections);
    }

    private static Section CreateSection(Column column)
    {
        var stringBuilder = new StringBuilder();
        foreach (var task in column.Tasks)
        {
            stringBuilder.Append(task.Title);
            if (!string.IsNullOrEmpty(task.FilePath))
            {
                stringBuilder.Append($" [{task.FilePath}]");
            }

            stringBuilder.Append(Environment.NewLine);
        }
        
        return new Kbn.Section(column.Title, stringBuilder.ToString());
    }
}