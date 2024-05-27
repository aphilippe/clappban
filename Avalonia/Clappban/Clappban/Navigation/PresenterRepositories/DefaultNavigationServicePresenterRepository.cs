using System;
using System.Collections.Generic;
using Clappban.ViewModels;

namespace Clappban.Navigation;

public class DefaultNavigationServicePresenterRepository : INavigationServicePresenterRepository
{
    private readonly Dictionary<Type, IViewModelPresenter> _presenterDictionary;

    public DefaultNavigationServicePresenterRepository(Dictionary<Type,IViewModelPresenter> presenterDictionary)
    {
        _presenterDictionary = presenterDictionary;
    }

    public IViewModelPresenter? GetPresenter(Type T)
    {
        if (!T.IsAssignableTo(typeof(ViewModelBase))) throw new ArgumentException($"Type {T} must be a {typeof(ViewModelBase)}");

        return _presenterDictionary.GetValueOrDefault(T);
    }
}