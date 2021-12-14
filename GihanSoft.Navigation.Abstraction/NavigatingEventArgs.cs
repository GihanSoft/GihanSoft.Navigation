namespace GihanSoft.Navigation.Abstraction;

/// <summary>
/// Arguments of Navigating event.
/// </summary>
public class NavigatingEventArgs : NavigatedEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigatingEventArgs"/> class.
    /// </summary>
    /// <param name="current">current page.</param>
    /// <param name="future">future page.</param>
    public NavigatingEventArgs(IPage current, IPage future)
        : base(current, future)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether navigation should be canceled or not.
    /// </summary>
    public bool Cancel { get; set; }
}