using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using EventManagement;
using NetworkWrapper;
using System.Runtime.CompilerServices;

public class NetworkEventTranslator : MonoBehaviour {

    Dictionary<string, Type> backendEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "generateRoomCode", typeof(GenerateRoomCodeArgs) },
        { "connectToServer", typeof(ConnectToServerArgs) },
        { "webClientDisconnect", typeof(WebClientDisconnectArgs) }
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

        Type type = backendEventTypeToArgs[args.messageType];
    
        object serverArgs = JsonConvert.DeserializeObject(args.messageData, type);
        var data = Convert.ChangeType(serverArgs, type);

        EventManager.Instance.triggerEvent(args.messageType, (EventArgs)data);
    }
}
