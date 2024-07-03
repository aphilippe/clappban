using System;
using Clappban.ViewModels;

namespace Clappban.Navigation.Navigators;

public class Navigator<TViewModel> : INavigator where TViewModel : ViewModelBase
{
    private readonly IViewModelPresenter _presenter;
    private readonly Func<TViewModel> _viewModelFactory;

    public Navigator(IViewModelPresenter presenter, Func<TViewModel> viewModelFactory)
    {
        _presenter = presenter;
        _viewModelFactory = viewModelFactory;
    }

    public void Navigate()
    {
        _presenter.Display(_viewModelFactory());
    }
}