﻿using System.Runtime.Serialization;

namespace GihanSoft.Navigation.WPF;

/// <summary>
/// Navigation Exception.
/// </summary>
[Serializable]
public class NavigationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class.
    /// </summary>
    public NavigationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class.
    /// </summary>
    /// <param name="message">message.</param>
    public NavigationException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class.
    /// </summary>
    /// <param name="message">message.</param>
    /// <param name="innerException">inner <see cref="Exception"/>.</param>
    public NavigationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected NavigationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}