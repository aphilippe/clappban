using System.Collections.Generic;

namespace Clappban.Kbn;

public class Kbn
{
    public Kbn(string title, IEnumerable<Section> sections)
    {
        Title = title;
        Sections = sections;
    }

    public string Title { get; set; }
    public IEnumerable<Section> Sections { get; }
}

public class Section
{
    public Section(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public string Title { get; }
    public string Content { get; set; }
}