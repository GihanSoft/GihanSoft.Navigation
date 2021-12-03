namespace GihanSoft.Navigation.Abstraction;

/// <summary>
/// Page interface to use as base of navigation pages.
/// </summary>
public interface IPage : IDisposable
{
    /// <summary>
    /// Gets title of page.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// refresh page. called after navigation and going back and forward.
    /// </summary>
    /// <returns>task of refresh.</returns>
    void Refresh();
}
