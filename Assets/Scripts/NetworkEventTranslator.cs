using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using EventManagement;
using NetworkWrapper;

public class NetworkEventTranslator : MonoBehaviour {

    Dictionary<string, Type> backendEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "generateRoomCode", typeof(GenerateRoomCodeArgs) },
        { "connectToServer", typeof(ConnectToServerArgs) },
        { "webClientDisconnect", typeof(WebClientDisconnectArgs) },
        { "connectToBackend", typeof(EventArgs)},
        { "networkError", typeof(NetworkErrorArgs)}
    };

    Dictionary<string, Type> unityEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "connectToServer/accept", typeof(GenerateRoomCodeArgs) },
        { "connectToServer/reject", typeof(ConnectToServerArgs) }
    };

    // Use this for initialization
    void Start() {
        registerForEvents();
    }

    void registerForEvents() {

        EventManager.Instance.registerCallbackForEvent("receiveMessageFromServer", onReceiveMessageFromServer);

        foreach (KeyValuePair<string, Type> entry in unityEventTypeToArgs) {

            EventCallback<EventArgs> callback = (EventArgs e) => {

                string data = JsonConvert.SerializeObject(e);
                EventManager.Instance.triggerEvent("sendMessageToServer", new MessageArgs { messageType = entry.Key, messageData = data });
            };

            EventManager.Instance.registerCallbackForEvent(entry.Key, callback);
        }
    }

    void onReceiveMessageFromServer(EventArgs e) {

        MessageArgs args = (MessageArgs)e;
        
        if(!backendEventTypeToArgs.ContainsKey(args.messageType)) {
            Debug.Log("BACKEND EVENT: " + args.messageType + " CANNOT BE TRANSLATED");
            return;
        }

        Type type = backendEventTypeToArgs[args.messageType];
        Debug.Log(args.messageData);
    
        object serverArgs = JsonConvert.DeserializeObject(args.messageData, type);
        var data = Convert.ChangeType(serverArgs, type);
        
        EventManager.Instance.triggerEvent(args.messageType, (EventArgs)data);
    }
}
