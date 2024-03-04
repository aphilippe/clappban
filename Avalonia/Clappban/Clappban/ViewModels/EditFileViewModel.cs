using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using Avalonia.Platform.Storage;
using Clappban.InjectionDependency;
using Clappban.Kbn.Builders;
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
    public EditFileViewModel(string filePath, IBoardRepository boardRepository)
    {
        using (var sr = new StreamReader(filePath))
        {
            Text = sr.ReadToEnd();
        }

        var saveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            ErrorDetected = false;
            if (IsTextValid(filePath, out var boardBuilder))
            {
                ErrorDetected = true;
                return;
            }

            // if (boardRepository != null)
            // {
            //     boardRepository.CurrentBoard = boardBuilder.Build();
            // }

            await SaveFile(filePath);

            Locator.Current.GetRequiredService<ModalManager>().CloseModal();
        });
        saveCommand.ThrownExceptions.Subscribe(ex => Console.Write(ex.Message));
        SaveCommand = saveCommand;

        CancelCommand = ReactiveCommand.Create(() => Locator.Current.GetRequiredService<ModalManager>().CloseModal());
    }

    private bool IsTextValid(string filePath, out IKbnBuilder kbnBuilder)
    {
        // boardBuilder = new BoardKbnBuilder(filePath);
        kbnBuilder = new DummyKbnBuilder();
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Text)));
        try
        {
            KbnFileReader.Read(streamReader, kbnBuilder);
        }
        catch (Exception)
        {
            return true;
        }

        return false;
    }

    private async Task SaveFile(string filePath)
    {
        var streamWriter = new StreamWriter(filePath);
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