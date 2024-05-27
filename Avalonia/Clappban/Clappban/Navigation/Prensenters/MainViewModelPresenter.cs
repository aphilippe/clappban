using System;
using Clappban.ViewModels;

namespace Clappban.Navigation;

public class MainViewModelPresenter : IViewModelPresenter
{
    public void Display(ViewModelBase viewModel)
    {
        CurrentViewModel = viewModel;
        CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler CurrentViewModelChanged;
    public ViewModelBase? CurrentViewModel { get; private set; }
}