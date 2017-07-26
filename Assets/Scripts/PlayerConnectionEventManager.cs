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

        var otherPlayers = connectionManager.getConnectedPlayerIds();

        ConnectToServerArgs args = (ConnectToServerArgs)e;
        connectionManager.connectPlayer(args.playerName, args.webClientSocketId);

        EventManager.Instance.triggerEvent("web/playerConnected", new WebPlayerConnectedArgs { messageTargets = otherPlayers, playerName = args.playerName });
    }

    void onPlayerDisconnected(EventArgs e) {
        WebClientDisconnectArgs args = (WebClientDisconnectArgs)e;
        
        string playerName = connectionManager.getPlayerById(args.webClientSocketId).playerName;
        connectionManager.disconnectPlayer(args.webClientSocketId);

        var otherPlayers = connectionManager.getConnectedPlayerIds();
        EventManager.Instance.triggerEvent("web/playerDisconnected", new WebPlayerDisconnectedArgs { messageTargets = otherPlayers, playerName = playerName });
    }
}
