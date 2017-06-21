using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGeneratorTest : MonoBehaviour {

    public int _playerId = 0;
    public List<int> _cards = new List<int>();

    const string remote = "wss://exploding-kittens-backend.herokuapp.com/";
    const string local = "ws://localhost:3000/";

    void Start() {

        EventManagement.EventManager.Instance.registerCallbackForEvent("ReceiveMessageFromServer",
            (System.EventArgs e) => {
                NetworkWrapper.ReceiveMessageFromServerArgs args = e as NetworkWrapper.ReceiveMessageFromServerArgs;
                Debug.Log(args.messageData);
            });

        EventManagement.EventManager.Instance.registerCallbackForEvent("startPlayerTurn",
            (System.EventArgs e) => {
                StartTurnArgs args = (StartTurnArgs)e;
                _playerId = args.playerId;
            });
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.A)) {
            EventManagement.EventManager.Instance.triggerEvent("StartConnectionToServer", new NetworkWrapper.StartConnectionToServerArgs() { url = "ws://localhost:3000/" });
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
