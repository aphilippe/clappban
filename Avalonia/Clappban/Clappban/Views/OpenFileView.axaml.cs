using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using Clappban.ViewModels;

namespace Clappban.Views;

public partial class OpenFileView : UserControl
{
    public OpenFileView()
    {
        InitializeComponent();
    }
    
    private async void OpenFileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false
        });

        if (!files.Any()) return;
        
        ((OpenFileViewModel)DataContext).ReadFileCommand.Execute(files[0]);
    }
}