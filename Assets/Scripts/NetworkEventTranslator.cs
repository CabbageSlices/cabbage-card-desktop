using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EventManagement;
using NetworkWrapper;

public class NetworkEventTranslator : MonoBehaviour {

    Dictionary<string, Type> backendEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "backend/generateRoomCode", typeof(GenerateRoomCodeArgs) },
        { "backend/connectToServer", typeof(ConnectToServerArgs) },
        { "backend/webClientDisconnect", typeof(WebClientDisconnectArgs) },
        { "connectToBackend", typeof(EventArgs)},
        { "networkError", typeof(NetworkErrorArgs)}
    };

    Dictionary<string, Type> unityBackendEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "backend/connectToServer/accept", typeof(GenerateRoomCodeArgs) },
        { "backend/connectToServer/reject", typeof(ConnectToServerArgs) }
    };

    Dictionary<string, Type> unityWebEventTypeToArgs = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase) {
        { "web/playerConnected", typeof(WebPlayerConnectedArgs) },
        { "web/playerDisconnected", typeof(WebPlayerDisconnectedArgs) }
    };

    // Use this for initialization
    void Start() {
        registerForEvents();
    }

    void registerForEvents() {

        EventManager.Instance.registerCallbackForEvent("receiveMessageFromServer", onReceiveMessageFromServer);
        //translateUnityEvents(unityBackendEventTypeToArgs, "sendMessageToServer", 8);
        //translateUnityEvents(unityWebEventTypeToArgs, "sendMessageToClient", 4);
        foreach (KeyValuePair<string, Type> entry in unityBackendEventTypeToArgs) {
            
            EventCallback<EventArgs> callback = (EventArgs e) => {

                string data = JsonConvert.SerializeObject(e);
                EventManager.Instance.triggerEvent("sendMessageToServer",
                    new MessageArgs { messageType = entry.Key.Remove(0, 8), messageData = data });
            };

            EventManager.Instance.registerCallbackForEvent(entry.Key, callback);
        }


        foreach (KeyValuePair<string, Type> entry in unityWebEventTypeToArgs) {

            EventCallback<EventArgs> callback = (EventArgs e) => {
                JObject message = JObject.FromObject(e);
                message.Add("messageType", entry.Key.Remove(0, 4));

                string data = message.ToString();
                EventManager.Instance.triggerEvent("sendMessageToServer",
                    new MessageArgs { messageType = "messageToClient", messageData = data });
            };

            EventManager.Instance.registerCallbackForEvent(entry.Key, callback);
        }
    }

    void translateUnityEvents(Dictionary<string, Type> eventMap, string networkEventToGenerate, int prefixLength) {
        foreach (KeyValuePair<string, Type> entry in eventMap) {

            EventCallback<EventArgs> callback = (EventArgs e) => {

                string data = JsonConvert.SerializeObject(e);
                EventManager.Instance.triggerEvent(networkEventToGenerate,
                    new MessageArgs { messageType = entry.Key.Remove(0, prefixLength), messageData = data });
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
    
        object serverArgs = JsonConvert.DeserializeObject(args.messageData, type);
        var data = Convert.ChangeType(serverArgs, type);
        
        EventManager.Instance.triggerEvent(args.messageType, (EventArgs)data);
    }
}
