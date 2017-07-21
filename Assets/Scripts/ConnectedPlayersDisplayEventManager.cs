using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

[RequireComponent(typeof(ConnectedPlayersDisplay))]
public class ConnectedPlayersDisplayEventManager : MonoBehaviour {

    public ConnectedPlayersDisplay playersDisplay;

    // Use this for initialization
    private void Start() {
        if (playersDisplay == null)
            playersDisplay = GetComponent<ConnectedPlayersDisplay>();

        subscribeToEvents();
    }

    void subscribeToEvents() {
        EventManager.Instance.registerCallbackForEvent("playerConnected", onPlayerConnected);
        EventManager.Instance.registerCallbackForEvent("playerDisconnected", onPlayerDisconnect);
    }

    public void onPlayerConnected(EventArgs e) {

        ConnectToServerArgs args = (ConnectToServerArgs)e;
        playersDisplay.addConnectedPlayer(args.playerName, args.webClientSocketId);
    }

    public void onPlayerDisconnect(EventArgs e) {

        WebClientDisconnectArgs args = (WebClientDisconnectArgs)e;
        playersDisplay.removeConnectedPlayer(args.webClientSocketId);
    }
}
