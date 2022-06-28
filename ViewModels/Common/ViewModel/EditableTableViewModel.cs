using System.Threading.Tasks;

namespace ViewModels.Common.ViewModel;

public abstract class EditableTableViewModel<M, T> : BaseViewModel where M : EditableTableModel<T>
    where T : Notifier
{
    protected EditableTableViewModel()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        UndoChangesCommand = new AsyncRelayCommand(UndoChanges);
    }

    public M Model { get; set; }

    public AsyncRelayCommand SaveCommand { get; }

    public AsyncRelayCommand UndoChangesCommand { get; }

    protected abstract Task Save();

    protected abstract Task UndoChanges();
}