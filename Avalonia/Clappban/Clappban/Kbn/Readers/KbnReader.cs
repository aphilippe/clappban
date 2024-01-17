using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Clappban.Kbn.Readers.LineReaders;

namespace Clappban.Kbn.Readers;

public class KbnReader
{
    private ILineReader _startLineReader;

    public KbnReader(ILineReader startLineReader)
    {
        _startLineReader = startLineReader ?? throw new ArgumentNullException(nameof(startLineReader));
    }


    public Kbn Read(StreamReader stream)
    {
        // Test possible line reader
        // get line
        // First returning true -> action
        // get next possible line readers
        // loop
        
        // get to the end of file or error
        // error when no line reader is valid

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
            lineReaders = chosenLineReader.NextPossibleReaders.ToList();
        }

        return new Kbn("", new Section[]{});
    }
    
    public class NotValidException : Exception {}
}