using Clappban.Models.Boards;
using Clappban.ViewModels;
using Splat;

namespace Clappban.InjectionDependency;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<IBoardRepository>(() => new BoardRepository());
        
        services.Register(() => new BoardViewModel(resolver.GetRequiredService<IBoardRepository>()));
        services.Register(() => new OpenFileViewModel(resolver.GetRequiredService<IBoardRepository>()));
        services.Register(() => new MainWindowViewModel(resolver.GetRequiredService<IBoardRepository>()));
    }
}