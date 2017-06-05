using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using EventManagement;

namespace NetworkWrapper {
    
    public class StartConnectionToServerArgs : EventArgs {
        public string url;
    }

    public class ReceiveMessageFromServerArgs : EventArgs {
        public string messageType;
        public string messageData;
    }

    /// <summary>
    /// This class is used as a wrapper around the underlying network library used to faciliate communication
    /// between this application and another server.
    /// The network agent works using an event messaging system. It uses the given event manager to listen for and generate
    /// events
    /// </summary>
    public class NetworkAgent : MonoBehaviour {
        
        private WebSocket socket;

        private void Start() {

            subscribeToEvents();
        }

        private void OnDestroy() {

            disconnect(new EventArgs());
        }

        private void subscribeToEvents() {

            EventManager.Instance.registerCallbackForEvent(EventManagement.EventType.StartConnectionToServer, connect);
            EventManager.Instance.registerCallbackForEvent(EventManagement.EventType.EndConnectionToServer, disconnect);
        }

        /// <summary>
        /// Attempt to connect to the server, args must contain the url to connect to
        /// Function assumes the SocketIO protocal is being used
        /// </summary>
        /// <param name="args">MUST be of type StartConnectionToServerArgs, contains url of the server to connect to</param>
        private void connect(EventArgs args) {

            StartConnectionToServerArgs connectionArgs = (StartConnectionToServerArgs)args;

            if (socket != null)
                socket.Close();

            //connect to the given url, append the socket.io protocal parameters
            socket = new WebSocket(connectionArgs.url + "/socket.io/?EIO=2&transport=websocket");
            socket.OnMessage += onMessage;

            socket.ConnectAsync();
        }

        /// <summary>
        /// Disconnect from the server if connected
        /// </summary>
        /// <param name="e"></param>
        private void disconnect(EventArgs e) {

            if (socket == null || !socket.IsAlive)
                return;

            socket.CloseAsync();
        }

        /// <summary>
        /// Callback for WebSocket.onMessage
        /// extracts the message type and data from the packet and sends it to the event pipeline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onMessage(object sender, MessageEventArgs e) {

            ReceiveMessageFromServerArgs args = new ReceiveMessageFromServerArgs() {
                messageType = extractMessageType(e.Data),
                messageData = extractMessageData(e.Data)
            };
            
            //ignore the session id message
            if(args.messageType == "sid")
                return;

            EventManager.Instance.triggerEvent(EventManagement.EventType.ReceiveMessageFromServer, args);
        }

        private string extractMessageType(string socketIOMessage) {

            //message type is the first string in the messsage
            //find text wrapped in quotes
            int posFirstQuote = socketIOMessage.IndexOf('\"');
            int posSecondQuote = socketIOMessage.IndexOf('\"', posFirstQuote + 1);

            //return the message without surrounding quotes
            return socketIOMessage.Substring(posFirstQuote + 1, posSecondQuote - posFirstQuote - 1);
        }

        private string extractMessageData(string socketIOMessage) {

            //it is assumed that all message data is sent as a single JSON object from the backend
            //that is, on the backend side if a message contains one or more parameters, they are all aggregated
            //into a single JSON object instead of being sent separately, individual peices of data should also
            //be wrapped in a JSON object
            //data in socketIO message comes after the message type, since all parameters are required to be aggregated
            //into a single JSON object we can grab everything between the first { and the last }
            int posStartBrace = socketIOMessage.IndexOf('{');
            int posEndBrace = socketIOMessage.LastIndexOf('}');

            //return the object including the { and }
            return socketIOMessage.Substring(posStartBrace, posEndBrace - posStartBrace + 1);
        }
    }
}