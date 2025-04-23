namespace Clappban.Kbn.Serializers.SectionExtractors;

public interface IKbnSectionInfoExtractorSelector
{
    IKbnSectionInfoExtractor<T>? Select<T>(T obj);
}