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
    public EditFileViewModel(string text, string filePath, INavigator finishEditingNavigator)
        : this (filePath, finishEditingNavigator)
    {
        Text = text;
    }
    
    public EditFileViewModel(string filePath, INavigator finishEditingNavigator)
    {
        FilePath = filePath;
        var saveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            ErrorDetected = false;
            if (!IsTextValid())
            {
                ErrorDetected = true;
                return;
            }

            await SaveFile(FilePath);

            finishEditingNavigator.Navigate();
        });
        saveCommand.ThrownExceptions.Subscribe(ex => Console.Write(ex.Message));
        SaveCommand = saveCommand;

        CancelCommand = ReactiveCommand.Create(finishEditingNavigator.Navigate);
    }

    private bool IsTextValid()
    {
        var kbnBuilder = new DummyKbnBuilder();
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(Text)));
        try
        {
            KbnFileReader.Read(streamReader, kbnBuilder);
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
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