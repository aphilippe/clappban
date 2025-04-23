using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clappban.Kbn.Serializers;
using Clappban.Kbn.Serializers.SectionExtractors;
using Moq;
using NUnit.Framework;
using Range = Moq.Range;

namespace Clappban.Tests.Models.Kbn.Serializers;

[TestFixture]
[TestOf(typeof(KbnSerializer<>))]
public class KbnSerializersTests
{
    
    [TestCase("title")]
    [TestCase("Other title")]
    public void Test_Serialize_WhenNoSections_ReturnKbnWithOnlyTitle(string title)
    {
        var expected = $"==== {title} ====" + Environment.NewLine;
        expected += Environment.NewLine;
        expected += "========" + Environment.NewLine;

        var kbnExtractor = Mock.Of<IKbnInfoExtractor<object>>();
        Mock.Get(kbnExtractor).Setup(x => x.GetTitle(It.IsAny<object>())).Returns(title);
        
        var sectionExtractorContainer = Mock.Of<IKbnSectionInfoExtractorSelector>();
        
        var serializer = new KbnSerializer<object>(kbnExtractor, sectionExtractorContainer);
        var result = serializer.Serialize(new object());
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Test_Serialize_WhenSectionSelectorDoNotFindForSection_ThrowsSectionExtractorNotFoundException()
    {
        var kbnExtractor = Mock.Of<IKbnInfoExtractor<object>>();
        Mock.Get(kbnExtractor).Setup(x => x.GetTitle(It.IsAny<object>())).Returns("title");
        Mock.Get(kbnExtractor).Setup(x => x.GetSections(It.IsAny<object>())).Returns(Enumerable.Range(0,2).Select(x => new object()));

        var sectionExtractorContainer = Mock.Of<IKbnSectionInfoExtractorSelector>();
        Mock.Get(sectionExtractorContainer).Setup(x => x.Select(It.IsAny<object>())).Returns<object>(null);
        
        var serializer = new KbnSerializer<object>(kbnExtractor, sectionExtractorContainer);
        
        Assert.Throws<SectionExtractorNotFoundException>(() => serializer.Serialize(new object()));
    }
    
    [Test]
    public void Test_Serialize_WhenThereAreSectionsOfSameType_ReturnValidKbn()
    {
        StringBuilder sb = new();
        sb.AppendLine($"==== title ====");
        sb.AppendLine();
        sb.AppendLine("==== Section 1 ====");
        sb.AppendLine();
        sb.AppendLine("content of section 1");
        sb.AppendLine();
        sb.AppendLine("==== Section 2 ====");
        sb.AppendLine();
        sb.AppendLine("content of section 2");
        sb.AppendLine();
        sb.AppendLine("========");
    
        var kbnExtractor = Mock.Of<IKbnInfoExtractor<object>>();
        Mock.Get(kbnExtractor).Setup(x => x.GetTitle(It.IsAny<object>())).Returns("title");
        Mock.Get(kbnExtractor).Setup(x => x.GetSections(It.IsAny<object>())).Returns(Enumerable.Range(0,2).Select(x => new object()));
        
        var sectionExtractor1 = Mock.Of<IKbnSectionInfoExtractor>();
        Mock.Get(sectionExtractor1).Setup(x => x.GetTitle()).Returns("Section 1");
        Mock.Get(sectionExtractor1).Setup(x => x.GetContent()).Returns("content of section 1");
        
        var sectionExtractor2 = Mock.Of<IKbnSectionInfoExtractor>();
        Mock.Get(sectionExtractor2).Setup(x => x.GetTitle()).Returns("Section 2");
        Mock.Get(sectionExtractor2).Setup(x => x.GetContent()).Returns("content of section 2");
        
        var sectionExtractorContainer = Mock.Of<IKbnSectionInfoExtractorSelector>();
        Mock.Get(sectionExtractorContainer).SetupSequence(x => x.Select(It.IsAny<object>()))
            .Returns(sectionExtractor1)
            .Returns(sectionExtractor2);
        
        var serializer = new KbnSerializer<object>(kbnExtractor, sectionExtractorContainer);
        var result = serializer.Serialize(new object());
        
        Assert.That(result, Is.EqualTo(sb.ToString()));
    }
}