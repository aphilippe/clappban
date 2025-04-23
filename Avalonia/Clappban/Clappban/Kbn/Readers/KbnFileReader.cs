using System;
using System.IO;
using System.Text.RegularExpressions;
using Clappban.Kbn.Builders;
using Clappban.Kbn.Readers.LineReaders;
using Clappban.Kbn.Readers.LineReaders.Actions;
using Clappban.Kbn.Readers.LineReaders.Conditions;

namespace Clappban.Kbn.Readers;

public static class KbnFileReader
{
    public static void Read(StreamReader stream, IKbnBuilder kbnBuilder)
    {
        var nothingAction = new DoNothingLineReaderAction();
        var setTitleAction = new LineReaderAction(x =>
        {
            var title = RetrieveTitle(x);
            kbnBuilder.SetTitle(title);
        });
        var addSectionAction = new LineReaderAction(x =>
        {
            var title = RetrieveTitle(x);
            kbnBuilder.AddSection(title);
        });
        var appendSectionContent = new LineReaderAction(x =>
        {
            kbnBuilder.LastSection().AppendToContent(x);
        });
        
        var titleCondition = new RegexLineReaderCondition(@"^={4} [^\s]+(\s+[^\s]+)* ={4}$");
        var emptyLineCondition = new EmptyLineReaderCondition();
        var alwaysTrueCondition = new AlwaysTrueLineReaderCondition();
        var endCondition = new RegexLineReaderCondition("={8}");

        var mainTitleLineReader = new ParameterizedLineReader(titleCondition, setTitleAction);
        var emptyLineAfterMainTitleLineReader = new ParameterizedLineReader(emptyLineCondition, nothingAction);
        var sectionTitleLineReader = new ParameterizedLineReader(titleCondition, addSectionAction);
        var emptyAfterSectionTitleLineReader = new ParameterizedLineReader(emptyLineCondition, nothingAction);
        var sectionContentLineReader = new ParameterizedLineReader(alwaysTrueCondition, appendSectionContent);
        var endLineReader = new ParameterizedLineReader(endCondition, nothingAction);

        mainTitleLineReader.NextPossibleReaders = new ILineReader[] { emptyLineAfterMainTitleLineReader };
        emptyLineAfterMainTitleLineReader.NextPossibleReaders = new ILineReader[] { sectionTitleLineReader, endLineReader };
        sectionTitleLineReader.NextPossibleReaders = new ILineReader[] { emptyAfterSectionTitleLineReader };
        emptyAfterSectionTitleLineReader.NextPossibleReaders = new ILineReader[]
            {sectionTitleLineReader, endLineReader, sectionContentLineReader};
        sectionContentLineReader.NextPossibleReaders = new ILineReader[] { sectionTitleLineReader, endLineReader, sectionContentLineReader };

        var fileReader = new FileReader(mainTitleLineReader);
        fileReader.Read(stream);
    }

    private static string RetrieveTitle(string line)
    {
        return line.Substring(5, line.Length - 10);
    }
}