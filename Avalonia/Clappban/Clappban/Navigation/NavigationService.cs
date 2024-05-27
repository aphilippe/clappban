using System;
using Clappban.InjectionDependency;
using Clappban.ViewModels;
using Splat;

namespace Clappban.Navigation;

public class NavigationService
{
    private readonly INavigationServicePresenterRepository _repository;
    private readonly IReadonlyDependencyResolver _resolver;

    public NavigationService(INavigationServicePresenterRepository repository, IReadonlyDependencyResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public void Display(Type viewModelType)
    {
        if (!viewModelType.IsAssignableTo(typeof(ViewModelBase))) throw new ArgumentException($"Type {viewModelType} must be a {typeof(ViewModelBase)}");

        var presenter = _repository.GetPresenter(viewModelType);
        if (presenter is null) throw new PresenterNotFoundException(viewModelType);

        var viewModel = (ViewModelBase) _resolver.GetRequiredService(viewModelType);
        if (viewModel is null) throw new Exception($"Can not create instance of {viewModelType}");
        
        presenter.Display(viewModel);
    }
}

public class PresenterNotFoundException : Exception
{
    public PresenterNotFoundException(Type viewModelType) 
        : base($"No Presenter found for type {viewModelType}") 
    {}
}