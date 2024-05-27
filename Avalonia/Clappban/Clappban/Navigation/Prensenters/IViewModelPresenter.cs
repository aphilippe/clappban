using System;
using Clappban.ViewModels;

namespace Clappban.Navigation;

public interface IViewModelPresenter
{
    void Display(ViewModelBase viewModelBase);
    ViewModelBase? CurrentViewModel { get; }
    event EventHandler CurrentViewModelChanged;
}