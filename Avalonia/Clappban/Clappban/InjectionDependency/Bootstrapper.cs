using System.IO;
using System.IO.Abstractions;
using Avalonia.Controls.Shapes;
using Clappban.Models.Boards;
using Clappban.Navigation;
using Clappban.Navigation.Navigators;
using Clappban.Navigation.Navigators.specifics;
using Clappban.Navigation.Navigators.Specifics.Task;
using Clappban.Utils.IdGenerators;
using Clappban.ViewModels;
using Clappban.ViewModels.Factories;
using Splat;

namespace Clappban.InjectionDependency;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        var mainViewPresenter = new MainViewModelPresenter();
        var modalViewPresenter = new ModalViewPresenter();
        
        services.RegisterLazySingleton<IBoardRepository>(() => new BoardRepository());

        // TODO uncomment and make it work
        services.Register<ITaskViewModelFactory>(() => new TaskViewModelFactory(modalViewPresenter, new CloseModalNavigator(modalViewPresenter)));
        
        services.Register<IColumnViewModelFactory>(() => new ColumnViewModelFactory(resolver.GetRequiredService<ITaskViewModelFactory>()));
        
        services.Register(() => new BoardViewModelFactory(
            resolver.GetRequiredService<IColumnViewModelFactory>(),
            new ParameterNavigator<string,EditFileViewModel>(modalViewPresenter, path => GenerateEditFileViewModel(path, modalViewPresenter)),
            resolver.GetRequiredService<IBoardRepository>())
        );
        
        services.Register(
            () => new OpenFileViewModel(
                resolver.GetRequiredService<IBoardRepository>(), 
            new ParameterNavigator<Board, BoardViewModel>(mainViewPresenter, 
                board => resolver.GetRequiredService<BoardViewModelFactory>().Create(board)
                )));
        
        services.Register(() => new MainWindowViewModel(modalViewPresenter, mainViewPresenter));
    }

    private static EditFileViewModel GenerateEditFileViewModel(string path, ModalViewPresenter modalViewPresenter)
    {
        string text;
        using (var sr = new StreamReader(path))
        {
             text = sr.ReadToEnd();
        }
        var finishEditingNavigator = new CloseModalNavigator(modalViewPresenter);
        return new EditFileViewModel(text, path, finishEditingNavigator);
    }
}