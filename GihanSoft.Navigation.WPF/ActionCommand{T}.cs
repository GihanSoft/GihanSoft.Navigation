using System.Windows.Input;

namespace GihanSoft.Navigation.WPF;

/// <summary>
/// ActionCommand with parameter.
/// </summary>
/// <typeparam name="T">input type.</typeparam>
internal class ActionCommand<T> : ICommand
{
    private readonly Action<T> action;
    private readonly Func<T, bool> canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCommand{T}"/> class.
    /// </summary>
    /// <param name="action">action to execute.</param>
    /// <param name="canExecute">function to check can execute.</param>
    public ActionCommand(Action<T> action, Func<T, bool> canExecute)
    {
        this.action = action ?? throw new ArgumentNullException(nameof(action));
        this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        if (parameter is T t)
        {
            return this.canExecute(t);
        }

        return false;
    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        if (parameter is T t)
        {
            this.action(t);
        }
    }

    /// <summary>
    /// Call on <see cref="CanExecute"/> changed.
    /// </summary>
    public void OnCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, new());
}