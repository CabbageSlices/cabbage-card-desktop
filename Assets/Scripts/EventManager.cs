using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagement;
using System;

namespace EventManagement {

    /// <summary>
    /// List of all possible types of events
    /// </summary>
    public enum EventType {
        SendMessageToServer,/// <Remark>Triggered when there is a message to send to server, args should contain the data to send</Remark>
        ReceiveMessageFromServer,/// <Remark>Triggered when there is a message arrived from the server, args contains the data recived</Remark>
        StartConnectionToServer,/// <Remark>Triggered when it is time to connect to the server, args should contain the url to connect to</Remark>
        EndConnectionToServer/// <Remark>Triggered when it is time to disconnect from the server, or the server disconnects</Remark>
    };

    /// <summary>
    /// Class that allows objects to listen for events, and trigger events
    /// Implemented as a Singleton for global access
    /// String parameter used as event type, this is NOT case sensitive so eventName == eventName == EVEnTNamE
    /// </summary>
    public class EventManager : IEventManager<string, EventArgs> {

        private Dictionary <string, EventCallback<EventArgs>> eventCallbacks;
        private static EventManager _instance;

        /// <summary>
        /// Accessor to the EventManager singleton
        /// </summary>
        public static EventManager Instance {
            get {
                if(_instance == null) {
                    _instance = new EventManager();
                }

                return _instance;
            }
        }

        public void registerCallbackForEvent(string eventType, EventCallback<EventArgs> callback) {
            
            eventType = eventType.ToLower();

            if(!Instance.eventCallbacks.ContainsKey(eventType))
                Instance.eventCallbacks.Add(eventType, null);

            Instance.eventCallbacks[eventType] += callback;
        }

        public void deregisterCallbackForEvent(string eventType, EventCallback<EventArgs> callback) {

            eventType = eventType.ToLower();

            if (Instance.eventCallbacks.ContainsKey(eventType))
                Instance.eventCallbacks[eventType] -= callback;
        }

        public void triggerEvent(string eventType, EventArgs args) {

            eventType = eventType.ToLower();

            if (Instance.eventCallbacks.ContainsKey(eventType) && Instance.eventCallbacks[eventType] != null) {
                //Debug.Log("triggered event " + eventType + " with a response");
                Instance.eventCallbacks[eventType](args);
            } else {
                //Debug.Log("triggered event " + eventType + "  WITHOUT a response");
            }
        }

        private EventManager() {
            eventCallbacks = new Dictionary<string, EventCallback<EventArgs>>();
        }
    }
}