using System;
using System.IO;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.Models.Boards;
using ReactiveUI;
using Console = System.Console;

namespace Clappban.ViewModels;

public class EditFileViewModel : ViewModelBase
{
    private readonly IBoardRepository _boardRepository;

    public EditFileViewModel(IStorageFile file, IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
        using (var sr = new StreamReader(file.OpenReadAsync().GetAwaiter().GetResult()))
        {
            Text = sr.ReadToEnd();
        }

        var saveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var stream = await file.OpenWriteAsync();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write(Text);
            streamWriter.Close();
            await _boardRepository.OpenAsync(file);
        });
        saveCommand.ThrownExceptions.Subscribe(ex => Console.Write(ex.Message));
        SaveCommand = saveCommand;
    }
    
    public string Text { get; set; }
    public ICommand SaveCommand { get; }
}