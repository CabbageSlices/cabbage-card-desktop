﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGeneratorTest : MonoBehaviour {
    
	void Start () {

        EventManagement.EventManager.Instance.registerCallbackForEvent("ReceiveMessageFromServer",
            (System.EventArgs e) => {
                NetworkWrapper.ReceiveMessageFromServerArgs args = e as NetworkWrapper.ReceiveMessageFromServerArgs;
                Debug.Log(args.messageData);
            });

    }
    
    void Update() {

        if (Input.GetKeyDown(KeyCode.A)) {
            EventManagement.EventManager.Instance.triggerEvent("StartConnectionToServer", new NetworkWrapper.StartConnectionToServerArgs() { url = "wss://exploding-kittens-backend.herokuapp.com/" });
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            EventManagement.EventManager.Instance.triggerEvent("EndConnectionToServer", null);
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            EventManagement.EventManager.Instance.triggerEvent("ShuffleDeck", null);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            EventManagement.EventManager.Instance.triggerEvent("DrawCard", new DrawCardArgs() {position = 0, playerId = 0});
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            EventManagement.EventManager.Instance.triggerEvent("useCardEffect/0", new CardEffectUseArgs { cardID=272 });
        }
    }
}
