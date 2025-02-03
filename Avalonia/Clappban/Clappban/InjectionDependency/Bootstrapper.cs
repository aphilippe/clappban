using Avalonia.Controls.Shapes;
using Clappban.Models.Boards;
using Clappban.Navigation;
using Clappban.Navigation.Navigators;
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

        services.Register<ITaskViewModelFactory>(() => new TaskViewModelFactory(
            // need to use conditionalNavigator here. 
            new ConditionalNavigator<Task>(
                task => !string.IsNullOrEmpty(task.FilePath),
                new ParameterNavigator<Task,EditFileViewModel>(modalViewPresenter, task => GenerateEditFileViewModel(task.FilePath, modalViewPresenter)),
                new NullNavigator<Task>()
            )
        ));
        
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
        var finishEditingNavigator = new CloseModalNavigator(modalViewPresenter);
        return new EditFileViewModel(path, finishEditingNavigator);
    }
}