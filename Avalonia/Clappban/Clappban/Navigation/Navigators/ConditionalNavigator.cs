using System;

namespace Clappban.Navigation.Navigators;

public class ConditionalNavigator <TParam> : INavigator<TParam>
{
    private readonly Func<TParam, bool> _condition;
    private readonly INavigator<TParam> _conditionalTrueNavigator;
    private readonly INavigator<TParam> _conditionalFalseNavigator;

    protected ConditionalNavigator(){}
    
    public ConditionalNavigator(Func<TParam, bool> condition, INavigator<TParam> conditionalTrueNavigator, INavigator<TParam> conditionalFalseNavigator)
    {
        _condition = condition;
        _conditionalTrueNavigator = conditionalTrueNavigator;
        _conditionalFalseNavigator = conditionalFalseNavigator;
    }

    public void Navigate(TParam task)
    {
        var navigator = _condition(task) ? _conditionalTrueNavigator : _conditionalFalseNavigator;
        navigator.Navigate(task);
    }
}