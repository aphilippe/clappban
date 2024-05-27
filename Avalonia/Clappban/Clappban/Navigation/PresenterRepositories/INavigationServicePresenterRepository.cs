using System;

namespace Clappban.Navigation;

public interface INavigationServicePresenterRepository
{
    IViewModelPresenter? GetPresenter(Type T);
}