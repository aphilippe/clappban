namespace Clappban.Kbn.Builders;

public interface IKbnBuilder
{
    void SetTitle(string title);
    void AddSection(string title);
    IKbnSectionBuilder LastSection();
}

public interface IKbnSectionBuilder
{
    void AppendToContent(string text);
}