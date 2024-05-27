using System;
using System.Collections.Generic;
using Clappban.Modal;
using Clappban.Models.Boards;
using Clappban.Navigation;
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
        
        var mainViewPresenter = new MainViewModelPresenter();
        services.Register<INavigationServicePresenterRepository>(() => new DefaultNavigationServicePresenterRepository(new Dictionary<Type, IViewModelPresenter>
        {
            {typeof(BoardViewModel), mainViewPresenter},
            {typeof(OpenFileViewModel), mainViewPresenter}
        }));
        services.RegisterLazySingleton(() => new NavigationService(resolver.GetRequiredService<INavigationServicePresenterRepository>(), resolver));
        
        services.Register(() => new BoardViewModel(resolver.GetRequiredService<IBoardRepository>(), resolver.GetRequiredService<ModalManager>()));
        services.Register(() => new OpenFileViewModel(resolver.GetRequiredService<IBoardRepository>(), resolver.GetRequiredService<NavigationService>()));
        services.Register(() => new MainWindowViewModel(resolver.GetRequiredService<IBoardRepository>(), modalViewModel, mainViewPresenter));
    }
}