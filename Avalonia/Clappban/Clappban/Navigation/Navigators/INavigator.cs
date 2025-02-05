namespace Clappban.Navigation.Navigators;

public interface INavigator
{
    void Navigate();
}

public interface INavigator<in TParam>
{
    void Navigate(TParam task);
}