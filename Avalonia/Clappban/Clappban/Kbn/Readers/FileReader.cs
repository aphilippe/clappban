using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Clappban.Kbn.Readers.LineReaders;

namespace Clappban.Kbn.Readers;

public class FileReader
{
    private ILineReader _startLineReader;

    public FileReader(ILineReader startLineReader)
    {
        _startLineReader = startLineReader ?? throw new ArgumentNullException(nameof(startLineReader));
    }

    public Kbn Read(StreamReader stream)
    {
        var lineReaders = new List<ILineReader> {_startLineReader};

        while (lineReaders.Any())
        {
            if (stream.EndOfStream)
            {
                throw new NotValidException();
            }
            
            var line = stream.ReadLine();
            var chosenLineReader = lineReaders.FirstOrDefault(x => x.IsValid(line));

            if (chosenLineReader == null)
            {
                throw new NotValidException();
            }
            
            chosenLineReader.Action(line);
            lineReaders = chosenLineReader.NextPossibleReaders?.ToList() ?? Enumerable.Empty<ILineReader>().ToList();
        }

        return new Kbn("", new Section[]{});
    }
    
    public class NotValidException : Exception {}
}