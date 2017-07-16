using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

//[RequireComponent(typeof(GameController))]
public class GameControllerEventManager : MonoBehaviour {

    public GameController controller;

	// Use this for initialization
	void Start () {

        if(controller == null)
            controller = GetComponent<GameController>();
        subscribeToEvents();
	}

    void subscribeToEvents() {

        EventManager.Instance.registerCallbackForEvent("generateRoomCode", onGenerateRoomCode);
    }
	
	void onGenerateRoomCode(EventArgs e) {
        GenerateRoomCodeArgs args = (GenerateRoomCodeArgs)e;

        //controller.setRoomCode(args.roomCode);
        EventManager.Instance.triggerEvent("setRoomCode", new SetRoomCodeArgs { roomCode = args.roomCode });
    }


}
