using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using Clappban.Kbn.Builders;
using Clappban.Kbn.Readers;
using Clappban.Navigation.Navigators;
using Microsoft.VisualBasic.CompilerServices;
using ReactiveUI;
using Console = System.Console;
using Task = System.Threading.Tasks.Task;

namespace Clappban.ViewModels;

public class EditFileViewModel : ViewModelBase
{
    public EditFileViewModel(string text, string filePath, INavigator finishEditingNavigator, ICommand? afterSaveCommand = null)
    {
        Text = text;
        FilePath = filePath;
        var validationCommand = ReactiveCommand.Create(() =>
        {
            ErrorDetected = false;
            ValidateText();
        });
        validationCommand.ThrownExceptions.Subscribe(ex =>
        {
            ErrorDetected = true;
            Console.Write(ex.Message);
        });
        
        var saveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await SaveFile(FilePath);
        });
        if (afterSaveCommand != null)
        {
           saveCommand.InvokeCommand(afterSaveCommand); 
        }

        validationCommand.InvokeCommand(saveCommand);
        
        validationCommand.Subscribe(ex => finishEditingNavigator.Navigate());
        SaveCommand = validationCommand;

        CancelCommand = ReactiveCommand.Create(finishEditingNavigator.Navigate);
    }

    private void ValidateText()
    {
        var kbnBuilder = new DummyKbnBuilder();
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Text)));
        KbnFileReader.Read(streamReader, kbnBuilder);
    }

    private async Task SaveFile(string filePath)
    {
        var streamWriter = new StreamWriter(filePath);
        await streamWriter.WriteAsync(Text);
        streamWriter.Close();
    }

    public string Text { get; set; } = "";
    public string FilePath { get; }
    private bool _errorDetected = false;
    public bool ErrorDetected
    {
        get => _errorDetected;
        private set => this.RaiseAndSetIfChanged(ref _errorDetected, value);
    }
    public ICommand SaveCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
}