using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Clappban.Kbn.Readers;
using Clappban.Kbn.Readers.LineReaders;
using Moq;
using NUnit.Framework;

namespace Clappban.Tests.Models.Kbn;

[TestFixture]
[TestOf(typeof(FileReader))]
public class FileReaderTest
{
    [Test]
    public void Test_Constructor_WhenStartLineReaderIsNull_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new FileReader(null));
    }
    
    [Test]
    public void Test_Read_WhenStartLineReaderReturnNotValid_ThrowNotValidException()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        using var sr = new StreamReader(stream);
        
        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

        FileReader reader = new FileReader(startLineReader);

        Assert.Throws<FileReader.NotValidException>(() => reader.Read(sr));
    }

    [Test]
    public void Test_Read_WhenStartLineReaderReturnValid_CallAction()
    {
        using var stream = GenerateStreamReader(1);

        var startLineReader = Mock.Of<ILineReader>();
        Mock.Get(startLineReader).Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        Mock.Get(startLineReader).Setup(x => x.Action(It.IsAny<string>()));

        FileReader reader = new FileReader(startLineReader);
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
        
        FileReader reader = new FileReader(startLineReader);
        reader.Read(stream);
        
        Mock.Get(startLineReader).Verify(x => x.NextPossibleReaders, Times.Once);
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
        
        var reader = new FileReader(startLineReader);

        Assert.Throws<FileReader.NotValidException>(() => reader.Read(stream));
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

        var reader = new FileReader(startLineReader);
        reader.Read(stream);

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
}