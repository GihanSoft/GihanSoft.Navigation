namespace GihanSoft.Navigation.Abstraction;

/// <summary>
/// Arguments of Navigated Event.
/// </summary>
public class NavigatedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigatedEventArgs"/> class.
    /// </summary>
    /// <param name="previous">previous page.</param>
    /// <param name="current">current page.</param>
    public NavigatedEventArgs(IPage previous, IPage current)
    {
        Previous = previous;
        Current = current;
    }

    /// <summary>
    /// Gets previous page.
    /// </summary>
    public IPage Previous { get; }

    /// <summary>
    /// Gets current page.
    /// </summary>
    public IPage Current { get; }
}