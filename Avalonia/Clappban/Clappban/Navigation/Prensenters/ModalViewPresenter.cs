using System;
using Clappban.ViewModels;

namespace Clappban.Navigation;

public class ModalViewPresenter : IViewModelPresenter
{
    private ViewModelBase? _currentViewModel;

    public void Close()
    {
        CurrentViewModel = null;
    }

    public void Display(ViewModelBase viewModelBase)
    {
        CurrentViewModel = viewModelBase;
    }

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        private set
        {
            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? CurrentViewModelChanged;
}