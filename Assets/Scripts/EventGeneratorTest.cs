using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class EventGeneratorTest : MonoBehaviour {

    public int _playerId = 0;
    public List<int> _cards = new List<int>();

    const string remote = "wss://exploding-kittens-backend.herokuapp.com/";
    const string local = "ws://localhost:3000/";

    void Start() {

        EventManagement.EventManager.Instance.registerCallbackForEvent("connectToServer", onConnectToServer);

        EventManagement.EventManager.Instance.registerCallbackForEvent("startPlayerTurn",
            (System.EventArgs e) => {
                StartTurnArgs args = (StartTurnArgs)e;
                _playerId = args.playerId;
            });
    }

    void onConnectToServer(System.EventArgs e) {
        
        //ssend the client type to the server
        JObject data = new JObject(new JProperty("clientType", "unity"));

        EventManagement.EventManager.Instance.triggerEvent("sendMessageToServer", new NetworkWrapper.MessageArgs() { messageType="unityClient", messageData=data.ToString() });
    }

    void onServerMessage(System.EventArgs e) {

        NetworkWrapper.MessageArgs args = e as NetworkWrapper.MessageArgs;

        if(args.messageType == "clientType") {

        }
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.A)) {
            EventManagement.EventManager.Instance.triggerEvent("StartConnectionToServer", new NetworkWrapper.StartConnectionToServerArgs() { url = remote });
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            EventManagement.EventManager.Instance.triggerEvent("EndConnectionToServer", null);
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            EventManagement.EventManager.Instance.triggerEvent("ShuffleDeck", null);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            EventManagement.EventManager.Instance.triggerEvent("DrawCard", new DrawCardArgs() { position = 0, playerId = _playerId });
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            EventManagement.EventManager.Instance.triggerEvent("useCardEffect/" + _playerId, new CardEffectUseArgs { cardID = 272 });
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            EventManagement.EventManager.Instance.triggerEvent("discardForRandomDraw/" + _playerId, new SelectCardsArgs() { cards = _cards.ToArray() });
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            EventManagement.EventManager.Instance.triggerEvent("selectTargetPlayer/done/" + _playerId, new SelectTargetPlayerResponseArgs() { playerBeingTargeted = 1 });
        }
    }
}
