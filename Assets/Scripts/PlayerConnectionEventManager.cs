using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

[RequireComponent(typeof(PlayerConnectionManager))]
public class PlayerConnectionEventManager : MonoBehaviour {

    public PlayerConnectionManager connectionManager;

	// Use this for initialization
	void Start () {
		if(connectionManager == null)
            connectionManager = GetComponent<PlayerConnectionManager>();

        subscribeToEvents();
	}

    void subscribeToEvents() {
        EventManager.Instance.registerCallbackForEvent("playerConnected", onPlayerConnected);
        EventManager.Instance.registerCallbackForEvent("playerDisconnected", onPlayerDisconnected);
    }

    void onPlayerConnected(EventArgs e) {
        ConnectToServerArgs args = (ConnectToServerArgs)e;
        connectionManager.connectPlayer(args.playerName, args.webClientSocketId);
    }

    void onPlayerDisconnected(EventArgs e) {
        WebClientDisconnectArgs args = (WebClientDisconnectArgs)e;
        connectionManager.disconnectPlayer(args.webClientSocketId);
    }
}
