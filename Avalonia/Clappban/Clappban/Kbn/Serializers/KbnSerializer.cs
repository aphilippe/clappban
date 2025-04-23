using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clappban.Kbn.Serializers.SectionExtractors;

namespace Clappban.Kbn.Serializers;

public class KbnSerializer<T> : IKbnSerializer<T>
{
    private readonly IKbnInfoExtractor<T> _kbnExtractor;
    
    private readonly IKbnSectionInfoExtractorSelector _sectionSelector;

    public KbnSerializer(IKbnInfoExtractor<T> kbnExtractor, IKbnSectionInfoExtractorSelector kbnSectionInfoExtractorSelector)
    {
        _kbnExtractor = kbnExtractor;
        _sectionSelector = kbnSectionInfoExtractorSelector;
    }

    public string Serialize(T obj)
    {
        StringBuilder sb = new();
        sb.AppendLine($"==== {_kbnExtractor.GetTitle(obj)} ====");
        sb.AppendLine();

        var sections = _kbnExtractor.GetSections(obj);
        foreach (var section in sections)
        {
            var sectionExtractor = _sectionSelector.Select(section);
            if (sectionExtractor is null)
            {
                throw new SectionExtractorNotFoundException(); 
            }

            sb.AppendLine($"==== {sectionExtractor.GetTitle(section)} ====");
            sb.AppendLine();
            sb.Append(sectionExtractor.GetContent(section));
            sb.AppendLine();
            sb.AppendLine();
        }
        // if (sections.Any() && _sectionInfoExtractors.Count == 0)
        // {
        //     throw new SectionExtractorNotFoundException();
        // }
        
        sb.AppendLine("========");
        
        return sb.ToString();
    }
}

public class SectionExtractorNotFoundException : Exception
{
    public SectionExtractorNotFoundException() 
        : base("Section extractor not found.")
    {
        
    }
}