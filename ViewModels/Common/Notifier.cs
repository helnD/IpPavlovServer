using System.ComponentModel;
using System.Runtime.CompilerServices;
using ViewModels.Annotations;

namespace ViewModels.Common;

public class Notifier : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
        OnPropertyChanged(propertyName);
    }
}