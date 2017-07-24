using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

[RequireComponent(typeof(GameController))]
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
        EventManager.Instance.registerCallbackForEvent("connectToServer", onWebConnectionRequest);
        EventManager.Instance.registerCallbackForEvent("webClientDisconnect", onWebClientDisconnect);
        EventManager.Instance.registerCallbackForEvent("startGame", onStartGame);
    }

    void onGenerateRoomCode(EventArgs e) {
        GenerateRoomCodeArgs args = (GenerateRoomCodeArgs)e;

        //Debug.Log(args.roomCode);
        controller.setRoomCode(args.roomCode);
        EventManager.Instance.triggerEvent("setRoomCode", new SetRoomCodeArgs { roomCode = args.roomCode });
    }

    void onWebConnectionRequest(EventArgs e) {

        ConnectToServerArgs args = (ConnectToServerArgs)e;

        if(!controller.isAwaitingConnection()) {
            EventManager.Instance.triggerEvent("connectToServer/reject", new ConnectToServerRejectArgs 
                { playerName = args.playerName, webClientSocketId = args.webClientSocketId, message = "Game is already in session." });
            return;
        }

        EventManager.Instance.triggerEvent("connectToServer/accept", new ConnectToServerAcceptArgs 
            { playerName = args.playerName, webClientSocketId = args.webClientSocketId });

        EventManager.Instance.triggerEvent("playerConnected", args);
    }

    void onWebClientDisconnect(EventArgs e) {
        WebClientDisconnectArgs args = (WebClientDisconnectArgs)e;

        EventManager.Instance.triggerEvent("playerDisconnected", args);
    }

    public void onStartGame(EventArgs e) {

        //createDeck();
        //dealCards();
        //hideConnectionScreen();
        //positionPlayerUi();
        //positionDeck();
        //determineStartingPlayer();
        
    }
}
