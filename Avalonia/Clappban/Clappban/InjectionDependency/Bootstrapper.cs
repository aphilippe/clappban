using Clappban.Modal;
using Clappban.Models.Boards;
using Clappban.ViewModels;
using Splat;

namespace Clappban.InjectionDependency;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IBoardRepository>(() => new BoardRepository());
        
        var modalViewModel = new ModalViewModel();
        services.RegisterLazySingleton(() => new ModalManager(modalViewModel));
        
        services.Register(() => new BoardViewModel(resolver.GetRequiredService<IBoardRepository>(), resolver.GetRequiredService<ModalManager>()));
        services.Register(() => new OpenFileViewModel(resolver.GetRequiredService<IBoardRepository>()));
        services.Register(() => new MainWindowViewModel(resolver.GetRequiredService<IBoardRepository>(), modalViewModel));
    }
}