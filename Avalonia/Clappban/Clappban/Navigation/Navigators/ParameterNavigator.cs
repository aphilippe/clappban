using System;
using Clappban.ViewModels;

namespace Clappban.Navigation.Navigators;

public class ParameterNavigator<TParam, TViewModel> : INavigator<TParam> where TViewModel : ViewModelBase
{
    private readonly IViewModelPresenter _presenter;
    private readonly Func<TParam, TViewModel> _viewModelFactory;

    public ParameterNavigator(IViewModelPresenter presenter, Func<TParam, TViewModel> viewModelFactory)
    {
        _presenter = presenter;
        _viewModelFactory = viewModelFactory;
    }

    public void Navigate(TParam param)
    {
        _presenter.Display(_viewModelFactory(param));
    }
}