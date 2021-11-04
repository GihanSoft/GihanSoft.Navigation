﻿// -----------------------------------------------------------------------
// <copyright file="AsyncEventHandler.cs" company="GihanSoft">
// Copyright (c) 2021 GihanSoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace GihanSoft.Navigation.Abstraction.Events
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the method that will handle an event when the event provides data.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">An object that contains the event data.</param>
    /// <returns>A task.</returns>
    public delegate Task EventHandlerAsync<in TEventArgs>(object sender, TEventArgs args);

    /// <summary>
    /// Represents the method that will handle an event when the event provides data.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender that generate event.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">An object that contains the event data.</param>
    /// <returns>A task.</returns>
    public delegate Task EventHandlerAsync<in TSender, in TEventArgs>(TSender sender, TEventArgs args);
}