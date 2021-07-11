using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.Common
{
    /// <summary>
    /// TPL based asynchronous relay command.
    /// </summary>
    public class AsyncRelayCommand : ICommand, IDisposable
    {
        private readonly Func<object, Task> execute;
        private readonly Func<object, bool> canExecute;
        private readonly Lazy<SemaphoreSlim> semaphore;
        private bool disposedValue;

        /// <summary>
        /// Raised when RaiseCanExecuteChanged is called.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Command action.</param>
        public AsyncRelayCommand(Func<object, Task> execute)
        {
            semaphore = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1));
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public AsyncRelayCommand(Func<Task> execute) : this(async o => await execute())
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Command action.</param>
        /// <param name="canExecute">Can execute an action.</param>
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute) : this(execute)
        {
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute) : this(async o => await execute(), o => canExecute())
        {
        }

        /// <summary>
        /// Determines whether this <see cref="AsyncRelayCommand"/> can execute in its current state.
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
        /// Executes the <see cref="AsyncRelayCommand"/> on the current command target.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public async void Execute(object parameter)
        {
            if (semaphore.Value.CurrentCount == 0)
            {
                return;
            }

            await semaphore.Value.WaitAsync();

            try
            {
                await execute(parameter);
            }
            finally
            {
                if (!disposedValue)
                {
                    semaphore.Value.Release();
                }
            }
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

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (semaphore.IsValueCreated)
                    {
                        semaphore.Value.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer.
                disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Relay command with parameter.
    /// </summary>
    /// <typeparam name="T">A parameter type.</typeparam>
    public class AsyncRelayCommand<T> : AsyncRelayCommand
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Command action.</param>
        public AsyncRelayCommand(Func<T, Task> execute)
            : base(async o =>
            {
                if (IsValidParameter(o))
                {
                    await execute((T)o);
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
        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
            : base(async o =>
            {
                if (IsValidParameter(o))
                {
                    await execute((T)o);
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
}