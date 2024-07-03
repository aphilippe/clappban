namespace Clappban.Navigation.Navigators;

public class CloseModalNavigator : INavigator
{
    private ModalViewPresenter _modalViewPresenter;

    public CloseModalNavigator(ModalViewPresenter modalViewPresenter)
    {
        _modalViewPresenter = modalViewPresenter;
    }

    public void Navigate()
    {
        _modalViewPresenter.Close();
    }
}