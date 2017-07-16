using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using EventManagement;
using System.Threading;

namespace NetworkWrapper {

    public class StartConnectionToServerArgs : EventArgs {
        public string url;
    }

    /// <summary>
    /// Arguments used to send message to server and receive emessage from server
    /// </summary>
    public class MessageArgs : EventArgs {
        public string messageType; ///<Remark>Typpe of message ebeing exchanged</Remark>
        public string messageData; ///<Remark>Event arguments sent to the server, converted to JSON using JSON.net</Remark>
    }

    /// <summary>
    /// This class is used as a wrapper around the underlying network library used to faciliate communication
    /// between this application and another server.
    /// The network agent works using an event messaging system. It uses the given event manager to listen for and generate
    /// events
    /// </summary>
    public class NetworkAgent : MonoBehaviour {

        /// <remark>
        /// how often to send the server a heart beat packet, in seconds
        /// </remark>
        public float heartbeatDelay = 10.0f;

        private WebSocket socket;

        List<MessageArgs> messageQueue = new List<MessageArgs>();

        Mutex mutex = new Mutex();

        private void Start() {

            subscribeToEvents();
        }

        private void Update() {
            
            mutex.WaitOne();

            foreach(MessageArgs args in messageQueue) {
                EventManager.Instance.triggerEvent("ReceiveMessageFromServer", args);
            }
            
            messageQueue.Clear();

            mutex.ReleaseMutex();
        }

        private void OnDestroy() {

            disconnect(new EventArgs());
        }

        private void subscribeToEvents() {

            EventManager.Instance.registerCallbackForEvent("StartConnectionToServer", connect);
            EventManager.Instance.registerCallbackForEvent("EndConnectionToServer", disconnect);
            EventManager.Instance.registerCallbackForEvent("sendMessageToServer", sendMessage);
        }

        /// <summary>
        /// Attempt to connect to the server, args must contain the url to connect to
        /// Function assumes the SocketIO protocal is being used
        /// </summary>
        /// <param name="args">MUST be of type StartConnectionToServerArgs, contains url of the server to connect to</param>
        public void connect(EventArgs args) {

            StartConnectionToServerArgs connectionArgs = (StartConnectionToServerArgs)args;

            if (socket != null)
                socket.Close();

            //connect to the given url, append the socket.io protocal parameters
            socket = new WebSocket(connectionArgs.url + "/socket.io/?EIO=2&transport=websocket");
            socket.OnMessage += onMessage;
            socket.OnOpen += onOpen;
            socket.OnError += onError;
            socket.OnClose += onClose;
            
            socket.ConnectAsync();
            StartCoroutine("sendHeartbeat");
        }

        /// <summary>
        /// Disconnect from the server if connected
        /// </summary>
        /// <param name="e"></param>
        public void disconnect(EventArgs e) {

            if (socket == null || !socket.IsAlive)
                return;

            socket.CloseAsync();
        }

        public void onError(object sender, ErrorEventArgs e) {
            EventManager.Instance.triggerEvent("networkError", new NetworkErrorArgs { errorMesssage = e.Message });
        }

        public void onClose(object sender, CloseEventArgs e) {
            EventManager.Instance.triggerEvent("connectionClosed", new ConnectionClosedArgs { message = e.Reason });
        }

        /// <summary>
        /// Send a message to the server if connected
        /// </summary>
        /// <param name="e">Event argument to send, must be of type MessageArgs</param>
        public void sendMessage(EventArgs e) {
            MessageArgs args = e as MessageArgs;

            string toSend = convertToSocketIoMessage(args.messageType, args.messageData);
            socket.Send(toSend);
        }

        IEnumerator sendHeartbeat() {

            while(true) {

                if(socket.IsAlive) {
                    string toSend = "2";
                    socket.Send(toSend);
                }

                yield return new WaitForSeconds(heartbeatDelay);
            }
        }

        private string convertToSocketIoMessage(string messageType, string messageDataJson) {

            //formatted to socket.io protocal
            return "42[\"" + messageType + "\"," + messageDataJson + "]";
        }

        private void onOpen(object sender, EventArgs e) {

            MessageArgs args = new MessageArgs() {
                messageType = "connectToBackend",
                messageData = ""
            };

            mutex.WaitOne();
            messageQueue.Add(args);
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// Callback for WebSocket.onMessage
        /// extracts the message type and data from the packet and sends it to the event pipeline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onMessage(object sender, MessageEventArgs e) {

            Debug.Log("Network Agent Received from backend: " + e.Data);

            if (e.Data == "40")
                return;

            MessageArgs args = new MessageArgs() {
                messageType = extractMessageType(e.Data),
                messageData = extractMessageData(e.Data)
            };

            //ignore the session id message
            if (args.messageType == "sid" || args.messageType == "")
                return;

            mutex.WaitOne();
            messageQueue.Add(args);
            mutex.ReleaseMutex();

            //EventManager.Instance.triggerEvent("ReceiveMessageFromServer", args);
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