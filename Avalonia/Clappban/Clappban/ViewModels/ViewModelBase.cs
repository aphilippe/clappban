using ReactiveUI;

namespace Clappban.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public virtual string Title => "Clappban";
}