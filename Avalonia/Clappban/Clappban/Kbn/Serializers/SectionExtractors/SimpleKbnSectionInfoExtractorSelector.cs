using System;

namespace Clappban.Kbn.Serializers.SectionExtractors;

public class SimpleKbnSectionInfoExtractorSelector<T> : IKbnSectionInfoExtractorSelector
{
    private readonly Func<T, IKbnSectionInfoExtractor> _factory;

    public SimpleKbnSectionInfoExtractorSelector(Func<T, IKbnSectionInfoExtractor> factory)
    {
        _factory = factory;
    }

    public IKbnSectionInfoExtractor Select(object obj)
    {
        return _factory((T)obj);
    }
}