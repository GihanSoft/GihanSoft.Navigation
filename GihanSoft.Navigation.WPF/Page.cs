using System.Windows;
using System.Windows.Controls;

using GihanSoft.Navigation.Abstraction;

namespace GihanSoft.Navigation.WPF;

/// <summary>
/// Page type to use as base of navigation pages.
/// </summary>
public class Page : UserControl, IPage
{
    private static void ThrowOnDisposed(DependencyObject d, DependencyPropertyChangedEventArgs _)
    {
        if (d is Page page && page.disposedValue)
        {
            throw new ObjectDisposedException(nameof(Page));
        }
    }

    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(Page),
        new PropertyMetadata(default(string), ThrowOnDisposed));

    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Page"/> class.
    /// </summary>
    protected Page()
    {
        this.SetValue(TitleProperty, this.GetType().Name);
    }

    /// <summary>
    /// Gets or sets title of page.
    /// </summary>
    public virtual string? Title
    {
        get => (string?)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    /// <summary>
    /// refresh page. called after navigation and going back and forward.
    /// </summary>
    /// <returns>task of refresh.</returns>
    public virtual void Refresh()
    {
        if (this.disposedValue)
        {
            throw new ObjectDisposedException(nameof(Page));
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// dispose page.
    /// </summary>
    /// <param name="disposing">true to dispose managed types.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // TO DO: dispose managed state (managed objects)
            }

            // TO DO: free unmanaged resources (unmanaged objects) and override finalizer
            // TO DO: set large fields to null
            this.disposedValue = true;
        }
    }
}