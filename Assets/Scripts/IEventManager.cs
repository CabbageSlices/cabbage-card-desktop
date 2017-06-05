using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventManagement {

    /// <summary>
    /// Function signature for callback functions that are executed when an event is triggered
    /// </summary>
    /// <typeparam name="CallbackArgumentType">The data type of the argument passed to the callback function. It must inherit from System.EventArgs </typeparam>
    /// <param name="args">the argument object containing information required for the callback</param>
    public delegate void EventCallback<CallbackArgumentType>(CallbackArgumentType args) where CallbackArgumentType : EventArgs;

    /// <summary>
    /// Interface that all event managers must implement.
    /// Generics are used to allow variations in the list of supported events, and in the type of data passed to all event handlers
    /// </summary>
    /// <typeparam name="EventType">Enum specifying all possible event types. Can't constrain to enum so struct is used as constraint instead</typeparam>
    /// <typeparam name="CallbackArgumentType">The data type of the argument passed to the callback function. It must inherit from System.EventArgs</typeparam>
    public interface IEventManager<EventType, CallbackArgumentType> 
        where EventType : struct
        where CallbackArgumentType : EventArgs {

        /// <summary>
        /// Register the given callback so that when an event of the given type of event is triggered, the given callback is executed
        /// </summary>
        /// <param name="eventType">Type of event to listen for</param>
        /// <param name="callback">Function to execute when the given event is triggered</param>
        void registerCallbackForEvent(EventType eventType, EventCallback<CallbackArgumentType> callback);

        /// <summary>
        /// Deregisters the given callback so that when an event of the given type is triggered, the given callback is no longer executed
        /// If the callback is registered to another type of event, then it will still be triggered by the other event.
        /// </summary>
        /// <param name="eventType">Type of event to stop listening for</param>
        /// <param name="callback">Function to be removed from the list of callbacks for the given event </param>
        void deregisterCallbackForEvent(EventType eventType, EventCallback<CallbackArgumentType> callback);

        /// <summary>
        /// Triggers the given type of event so that all callbacks registered for that event type are exectued
        /// All events will be passed args as a parameter
        /// </summary>
        /// <param name="eventType">Type of event to trigger</param>
        /// <param name="args">arguments for each callback listening for the given event</param>
        void triggerEvent(EventType eventType, CallbackArgumentType args);
    }
}