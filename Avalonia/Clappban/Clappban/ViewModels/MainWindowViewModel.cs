using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Clappban.Kbn;
using Clappban.Kbn.Readers;
using Clappban.Kbn.Readers.LineReaders;
using Clappban.Kbn.Readers.LineReaders.Actions;
using Clappban.Kbn.Readers.LineReaders.Conditions;
using Clappban.Models.Boards;
using ReactiveUI;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ReadFileCommand = ReactiveCommand.CreateFromTask<IStorageFile>(OpenFileAsync);
    }

    private async Task OpenFileAsync(IStorageFile file)
    {
        await using var stream = await file.OpenReadAsync();
        using var streamReader = new StreamReader(stream);

        var boardBuilder = new BoardKbnBuilder();
        
        KbnFileReader.Read(streamReader, boardBuilder);

        var board = boardBuilder.Build();
    }

    public ICommand ReadFileCommand { get; }
}