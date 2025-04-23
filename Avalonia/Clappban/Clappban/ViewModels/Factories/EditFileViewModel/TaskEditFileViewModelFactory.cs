using System.IO.Abstractions;
using Clappban.Models.Boards;
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

            // var kbnString = kbnSerializer.Serialize(board); // Specific serializer for board
            // var filePath = board.FilePath + "test";
            // _fileSystem.File.WriteAllText(filePath, kbnString);
            
            // old
            // var kbn = BoardToKbnFactory.CreateKbn(board);
            // var kbnWriter = new KbnWriter();
            // using var fileStream = new StreamWriter(board.FilePath + "test");
            // kbnWriter.Write(kbn,fileStream);
        });
        
        return new ViewModels.EditFileViewModel(filePath, _finishEditingNavigator, afterSaveCommand);
    }
}