using System.Windows.Input;

namespace GihanSoft.Navigation.WPF;

/// <summary>
/// A simple command to use internally.
/// </summary>
internal class ActionCommand : ICommand
{
    private readonly Action action;
    private readonly Func<bool> canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCommand"/> class.
    /// </summary>
    /// <param name="action">action to execute.</param>
    /// <param name="canExecute">function to check can execute.</param>
    public ActionCommand(Action action, Func<bool> canExecute)
    {
        this.action = action ?? throw new ArgumentNullException(nameof(action));
        this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc />
    public bool CanExecute(object? parameter) => this.canExecute();

    /// <inheritdoc />
    public void Execute(object? parameter) => this.action();

    /// <summary>
    /// Call on <see cref="CanExecute"/> changed.
    /// </summary>
    public void OnCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, new());
}