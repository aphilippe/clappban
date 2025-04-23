namespace Clappban.Kbn.Serializers.SectionExtractors;

public interface IKbnSectionInfoExtractorSelector
{
    IKbnSectionInfoExtractor? Select(object obj);
}