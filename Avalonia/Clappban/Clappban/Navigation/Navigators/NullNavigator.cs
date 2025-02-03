namespace Clappban.Navigation.Navigators;

public class NullNavigator : INavigator
{
    public void Navigate()
    {
        // do nothing
    }
}

public class NullNavigator<TParam> : INavigator<TParam>
{
    public void Navigate(TParam param)
    {
        // Do nothing
    }
}