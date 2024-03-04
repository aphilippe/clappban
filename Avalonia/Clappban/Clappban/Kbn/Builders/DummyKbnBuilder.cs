namespace Clappban.Kbn.Builders;

public class DummyKbnBuilder : IKbnBuilder
{
    private IKbnSectionBuilder _sectionBuilder = new DummyKbnSectionBuilder();
    public void SetTitle(string title)
    {
        // do nothing
    }

    public void AddSection(string title)
    {
        // do Nothing
    }

    public IKbnSectionBuilder LastSection()
    {
        return _sectionBuilder;
    }
}

public class DummyKbnSectionBuilder : IKbnSectionBuilder
{
    public void AppendToContent(string text)
    {
        // do nothing
    }
}