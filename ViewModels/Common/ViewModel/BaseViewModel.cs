using System.Threading.Tasks;

namespace ViewModels.Common.ViewModel;

public abstract class BaseViewModel : Notifier
{
    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}