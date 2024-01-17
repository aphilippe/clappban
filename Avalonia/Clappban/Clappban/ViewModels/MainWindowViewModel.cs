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
using Clappban.Models.Boards;
using ReactiveUI;

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
        var boardReader = new KbnReader(null);
        boardReader.Read(new StreamReader(stream));
        
        
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();
        
        Console.Write(fileContent);
    }

    public ICommand ReadFileCommand { get; }
}