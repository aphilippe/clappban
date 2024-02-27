using Clappban.ViewModels;

namespace Clappban.Modal;

public class ModalManager
{
    private ModalViewModel _modalViewModel;

    public ModalManager(ModalViewModel modalViewModel)
    {
        _modalViewModel = modalViewModel;
    }
    
    public void DisplayModal(ViewModelBase viewModel)
    {
        _modalViewModel.CurrentViewModel = viewModel;
    }

    public void CloseModal()
    {
        _modalViewModel.CurrentViewModel = null;
    }
}