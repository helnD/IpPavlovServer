using System;
using System.Windows.Input;

namespace ViewModels.Common;

/// <summary>
/// A command whose sole purpose is to relay its functionality
/// to other objects by invoking delegates.
/// The default return value for the CanExecute method is 'true'.
/// <see cref="RaiseCanExecuteChanged"/> needs to be called whenever
/// <see cref="CanExecute"/> is expected to return a different value.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object> execute;
    private readonly Func<object, bool> canExecute;

    /// <summary>
    /// Raised when RaiseCanExecuteChanged is called.
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute">Command action.</param>
    public RelayCommand(Action<object> execute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    /// <summary>
    /// Creates a new command that can always execute.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action execute) : this(o => execute())
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute">Command action.</param>
    /// <param name="canExecute">Can execute an action.</param>
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
    {
        this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
    {
    }

    /// <summary>
    /// Determines whether this <see cref="RelayCommand"/> can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
    /// </param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    /// <summary>
    /// Executes the <see cref="RelayCommand"/> on the current command target.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
    /// </param>
    public void Execute(object parameter)
    {
        execute(parameter);
    }

    /// <summary>
    /// Method used to raise the <see cref="CanExecuteChanged"/> event
    /// to indicate that the return value of the <see cref="CanExecute"/>
    /// method has changed.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Relay command with parameter.
/// </summary>
/// <typeparam name="T">A parameter type.</typeparam>
public class RelayCommand<T> : RelayCommand
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute">Command action.</param>
    public RelayCommand(Action<T> execute)
        : base(o =>
        {
            if (IsValidParameter(o))
            {
                execute((T)o);
            }
        })
    {
        if (execute == null)
        {
            throw new ArgumentNullException(nameof(execute));
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute">Command action.</param>
    /// <param name="canExecute">Can execute an action.</param>
    public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        : base(o =>
        {
            if (IsValidParameter(o))
            {
                execute((T)o);
            }
        }, o => IsValidParameter(o) && canExecute((T)o))
    {
        if (execute == null)
        {
            throw new ArgumentNullException(nameof(execute));
        }

        if (canExecute == null)
        {
            throw new ArgumentNullException(nameof(canExecute));
        }
    }

    static bool IsValidParameter(object o)
    {
        if (o != null)
        {
            // The parameter isn't null, so we don't have to worry whether null is a valid option
            bool isValid = o is T;
#if DEBUG
            if (!isValid)
            {
                throw new ArgumentException($"The type of parameter ({o.GetType()}) is a mismatch the type of the command ({typeof(T)})");
            }
#endif

            return isValid;
        }

        var t = typeof(T);

        // The parameter is null. Is T Nullable?
        if (Nullable.GetUnderlyingType(t) != null)
        {
            return true;
        }

        // Not a Nullable, if it's a value type then null is not valid
        return t.GetType().IsValueType;
    }
}