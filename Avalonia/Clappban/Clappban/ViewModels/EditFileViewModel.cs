using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Kbn.Readers;
using Clappban.Modal;
using Clappban.Models.Boards;
using ReactiveUI;
using Splat;
using Console = System.Console;
using Task = System.Threading.Tasks.Task;

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
            ErrorDetected = false;
            if (IsTextValid(file, out var boardBuilder))
            {
                ErrorDetected = true;
                return;
            }

            _boardRepository.CurrentBoard = boardBuilder.Build();
            
            await SaveFile(file);

            Locator.Current.GetRequiredService<ModalManager>().CloseModal();
        });
        saveCommand.ThrownExceptions.Subscribe(ex => Console.Write(ex.Message));
        SaveCommand = saveCommand;

        CancelCommand = ReactiveCommand.Create(() => Locator.Current.GetRequiredService<ModalManager>().CloseModal());
    }

    private bool IsTextValid(IStorageFile file, out BoardKbnBuilder boardBuilder)
    {
        boardBuilder = new BoardKbnBuilder(file);
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Text)));
        try
        {
            KbnFileReader.Read(streamReader, boardBuilder);
        }
        catch (Exception)
        {
            return true;
        }

        return false;
    }

    private async Task SaveFile(IStorageFile file)
    {
        var stream = await file.OpenWriteAsync();
        var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteAsync(Text);
        streamWriter.Close();
    }

    public string Text { get; set; }
    private bool _errorDetected = false;
    public bool ErrorDetected
    {
        get => _errorDetected;
        private set => this.RaiseAndSetIfChanged(ref _errorDetected, value);
    }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
}