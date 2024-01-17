using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clappban.Kbn;
using Clappban.Kbn.Readers;
using Clappban.Kbn.Readers.LineReaders;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Clappban.Tests.Models.Kbn;

[TestFixture]
[TestOf(typeof(KbnReader))]
public class KbnReaderTest
{
    [Test]
    public void Test_Constructor_WhenStartLineReaderIsNull_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new KbnReader(null));
    }
    
    [Test]
    public void Test_Read_WhenStartLineReaderReturnNotValid_ThrowNotValidException()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        using var sr = new StreamReader(stream);
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

        KbnReader reader = new KbnReader(startLineReader);

        Assert.Throws<KbnReader.NotValidException>(() => reader.Read(sr));
    }

    [Test]
    public void Test_Read_WhenStartLineReaderReturnValid_CallAction()
    {
        using var stream = GenerateStreamReader(1);

        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));

        KbnReader reader = new KbnReader(startLineReader);
        reader.Read(stream);
        
        Mock.Get(startLineReader).Verify(x => x.Action(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Test_Read_WhenStartLineReaderIsValid_CallNextPossibleReader()
    {
        using var stream = GenerateStreamReader(1);
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));
        Mock.Get(startLineReader).Setup(x => x.NextPossibleReaders);
        
        KbnReader reader = new KbnReader(startLineReader);
        reader.Read(stream);
        
        Mock.Get(startLineReader).Verify(x => x.NextPossibleReaders, Times.Once);
    }

    [Test]
    public void Test_Read_WhenStartLineReaderIsValidAndNextPossibleReadersIsEmpty_ReturnKbn()
    {
        using var stream = GenerateStreamReader(1);
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));
        Mock.Get(startLineReader).Setup(x => x.NextPossibleReaders).Returns(Enumerable.Empty<ILineReader>());

        var reader = new KbnReader(startLineReader);
        var kbn = reader.Read(stream);

        Assert.That(kbn, Is.Not.Null);
    }

    [Test]
    public void
        Test_Read_WhenStartLineReaderIsValidAndHasPossibleReadersAndStreamIsFiniched_ThrowNotValidFileException()
    {
        using var stream = GenerateStreamReader(1);

        var reader1 = Mock.Of<ILineReader>();
        
        var nextReaders = new List<ILineReader>
        {
            reader1
        };
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));
        Mock.Get(startLineReader).Setup(x => x.NextPossibleReaders).Returns(nextReaders);
        
        var reader = new KbnReader(startLineReader);

        Assert.Throws<KbnReader.NotValidException>(() => reader.Read(stream));
    }

    [Test]
    public void Test_Read_WhenStartLienReaderIsValidAndHasNextPossibleReaders_CallActionOnTheFirstValid()
    {
        using var stream = GenerateStreamReader(2);

        var reader1 = Mock.Of<ILineReader>();
        Mock.Get(reader1).Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
        
        var reader2 = Mock.Of<ILineReader>();
        Mock.Get(reader2).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(reader2).Setup(x => x.Action(It.IsAny<string>()));
        
        var reader3 = Mock.Of<ILineReader>();
        Mock.Get(reader3).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(reader3).Setup(x => x.Action(It.IsAny<string>()));

        var nextReaders = new List<ILineReader>
        {
            reader1, reader2, reader3
        };
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));
        Mock.Get(startLineReader).Setup(x => x.NextPossibleReaders).Returns(nextReaders);

        var reader = new KbnReader(startLineReader);
        var kbn = reader.Read(stream);

        Mock.Get(reader1).Verify(x => x.Action(It.IsAny<string>()), Times.Never());
        Mock.Get(reader1).Verify(x => x.NextPossibleReaders, Times.Never());
        Mock.Get(reader3).Verify(x => x.Action(It.IsAny<string>()), Times.Never());
        Mock.Get(reader1).Verify(x => x.NextPossibleReaders, Times.Never());
        
        Mock.Get(reader2).Verify(x => x.Action(It.IsAny<string>()), Times.Once());
        Mock.Get(reader2).Verify(x => x.NextPossibleReaders, Times.Once());
    }

    private StreamReader GenerateStreamReader(int lineNumber)
    {
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < lineNumber; i++)
        {
            stringBuilder.AppendLine($"line {i+1}");
        }

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
        return new StreamReader(stream);
    }
    
// With refactoring kbn reader is just a line reader launcher
//     [Test]
//     public void Test_Read_WhenStreamIsEmpty_ThrowFileNotValidException()
//     {
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes(""));
//         using var sr = new StreamReader(stream);
//         
//         KbnReader reader = new KbnReader();
//         Assert.Throws<KbnReader.NotValidException>(() => reader.Read(new StreamReader(stream)));
//     }
//
//     [Test]
//     public void Test_Read_WhenStreamFirstLineIsNotValid_ThrowFileNotValidException()
//     {
//         using var stream = new MemoryStream("afsd0"u8.ToArray());
//         using var sr = new StreamReader(stream);
//         
//         KbnReader reader = new KbnReader();
//         Assert.Throws<KbnReader.NotValidException>(() => reader.Read(new StreamReader(stream)));
//     }
//     
// #region Title
//
//     [TestCase("Title 1")]
//     [TestCase("Title 2")]
//     [TestCase("Title 3")]
//     [TestCase("Title with special c@racters")]
//     public void Test_Read_WhenOnlyOneValidLine_ReturnKbnWithTitleSet(string title)
//     {
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes($"==== {title} ===="));
//         using var sr = new StreamReader(stream);
//         
//         KbnReader reader = new KbnReader();
//         var result = reader.Read(sr);
//         
//         ClassicAssert.NotNull(result);
//         Assert.That(result.Title, Is.EqualTo(title));
//     }
//     
//     [TestCase("=== 3 line start ====")]
//     [TestCase("==== 3 line end ===")]
//     [TestCase("====  double space before ====")]
//     [TestCase("==== double space after  ====")]
//     [TestCase("a==== line don't start with =  ====")]
//     public void Test_Read_WhenOnlyOneNotValidLine_ThrowNotValidException(string line)
//     {
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes(line));
//         using var sr = new StreamReader(stream);
//         
//         KbnReader reader = new KbnReader();
//         Assert.Throws<KbnReader.NotValidException>(() => reader.Read(sr));
//     }
//     
// #endregion
//
// #region Sections
//
//     [Test]
//     public void Test_Read_WhenThereIsNoSection_ReturnKbnWithNoSection()
//     {
//         var stringBuilder = new StringBuilder();
//         stringBuilder.AppendLine("==== Title ====");
//         
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
//         using var sr = new StreamReader(stream);
//         
//         var reader = new KbnReader();
//
//         var result = reader.Read(sr);
//         
//         Assert.That(result, Is.Not.Null);
//         Assert.That(result.Title, Is.EqualTo("Title"));
//         Assert.That(result.Sections, Is.Empty);
//     }
//
//     [Test]
//     public void Test_Read_WhenOneSectionExist_ReturnKbnWithOneSection()
//     {
//         var stringBuilder = new StringBuilder();
//         stringBuilder.AppendLine("==== Title ====");
//         stringBuilder.AppendLine();
//         stringBuilder.AppendLine("==== Section ====");
//         
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
//         using var sr = new StreamReader(stream);
//
//         var reader = new KbnReader();
//
//         var result = reader.Read(sr);
//         
//         Assert.That(result, Is.Not.Null);
//         Assert.That(result.Title, Is.EqualTo("Title"));
//         Assert.That(result.Sections, Has.Exactly(1).Items);
//         Assert.That(result.Sections, Has.Exactly(1).Matches<Section>(x => x.Title == "Section"));
//     }
//
//     [Test]
//     public void Test_Read_WhenThereAreSections_ReturnKbnWithAllSections()
//     {
//         var stringBuilder = new StringBuilder();
//         stringBuilder.AppendLine("==== Title ====");
//         stringBuilder.AppendLine();
//         stringBuilder.AppendLine("==== Section 1 ====");
//         stringBuilder.AppendLine();
//         stringBuilder.Append("Value for Section 1");
//         stringBuilder.AppendLine();
//         stringBuilder.AppendLine("==== Section 2 ====");
//         stringBuilder.AppendLine();
//         stringBuilder.Append("Value for Section 2");
//         stringBuilder.AppendLine();
//         stringBuilder.AppendLine("==== Section 3 ====");
//         stringBuilder.AppendLine();
//         stringBuilder.Append("Value for Section 3");
//         
//         using var stream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
//         using var sr = new StreamReader(stream);
//
//         var reader = new KbnReader();
//
//         var result = reader.Read(sr);
//         
//         Assert.That(result.Title, Is.EqualTo("Title"));
//         Assert.That(result.Sections, Has.Exactly(1).Matches<Section>(x => x.Title == "Section 1" && x.Content == "Value for Section 1")
//             .And.Exactly(1).Matches<Section>(x => x.Title == "Section 2" && x.Content == "Value for Section 2")
//             .And.Exactly(1).Matches<Section>(x => x.Title == "Section 3" && x.Content == "Value for Section 3"));
//     }
//
// #endregion
}