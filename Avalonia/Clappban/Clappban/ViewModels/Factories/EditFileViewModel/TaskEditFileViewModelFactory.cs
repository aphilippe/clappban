using System.IO;
using System.IO.Abstractions;
using System.Text;
using Clappban.Kbn.Serializers;
using Clappban.Kbn.Serializers.SectionExtractors;
using Clappban.Models.Boards;
using Clappban.Models.Boards.Serialization;
using Clappban.Navigation.Navigators;
using ReactiveUI;

namespace Clappban.ViewModels.Factories.EditFileViewModel;

public class TaskEditFileViewModelFactory
{
    private readonly INavigator _finishEditingNavigator;
    private readonly IFileSystem _fileSystem;
    private readonly ITaskFilePathGenerator _filePathGenerator;

    public TaskEditFileViewModelFactory(INavigator finishEditingNavigator,
        IFileSystem fileSystem,
        ITaskFilePathGenerator filePathGenerator)
    {
        _finishEditingNavigator = finishEditingNavigator;
        _fileSystem = fileSystem;
        _filePathGenerator = filePathGenerator;
    }
    
    public ViewModels.EditFileViewModel Create(Task task, Board board)
    {
        if (!string.IsNullOrWhiteSpace(task.FilePath))
        {
            var text = _fileSystem.File.ReadAllText(task.FilePath);
            return new ViewModels.EditFileViewModel(text, task.FilePath, _finishEditingNavigator);
        }
        
        var filePath = _filePathGenerator.Generate(task);

        var afterSaveCommand = ReactiveCommand.Create(() =>
        {
            task.FilePath = filePath;

            var boardInfoExtractor = new BoardKbnInfoExtractor();
            var selector = new SimpleKbnSectionInfoExtractorSelector<Column>(c => new ColumnKbnSectionInfoExtractor(c, new TaskSerializer(Path.GetDirectoryName(board.FilePath))));
            var boardKbnSerializer = new KbnSerializer<Board>(boardInfoExtractor, selector);
            var boardString = boardKbnSerializer.Serialize(board);
            _fileSystem.File.WriteAllText(board.FilePath, boardString);
        });

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"==== {task.Title} ====")
            .AppendLine()
            .AppendLine("========");
        
        return new ViewModels.EditFileViewModel(stringBuilder.ToString(), filePath, _finishEditingNavigator, afterSaveCommand);
    }
}