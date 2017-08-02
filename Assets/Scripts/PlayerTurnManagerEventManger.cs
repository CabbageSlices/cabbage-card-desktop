using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

/// <summary>
/// Listen for and respond to events for the player turn manager
/// generate events from the player turn manager
/// </summary>
[RequireComponent(typeof(PlayerTurnManager))]
public class PlayerTurnManagerEventManger : MonoBehaviour {
    
    PlayerTurnManager turnManager;

    private void Start() {
        if(turnManager == null)
            turnManager = GetComponent<PlayerTurnManager>();
        subscribeToEvents();
    }

    void subscribeToEvents() {
        EventManager.Instance.registerCallbackForEvent("endPlayerTurn", onPlayerEndTurn);
        EventManager.Instance.registerCallbackForEvent("playerDeath", onPlayerDeath);
        EventManager.Instance.registerCallbackForEvent("startFirstPlayerTurn", onStartFirstPlayerTurn);
    }

    public void onPlayerEndTurn(EventArgs e) {

        //EndTurnArgs args = (EndTurnArgs) e;
        string idNextPlayer = turnManager.startNextPlayerTurn();

        triggerStartTurnEvent(idNextPlayer, turnManager.getNameCurrentPlayer());
    }

    public void onPlayerDeath(EventArgs e) {

        PlayerDeathArgs args = (PlayerDeathArgs)e;

        turnManager.onPlayerDeath(args.playerId);
    }

    public void onStartFirstPlayerTurn(EventArgs e) {
        triggerStartTurnEvent(turnManager.getIdCurrentPlayer(), turnManager.getNameCurrentPlayer());
    }

    private void triggerStartTurnEvent(string playerId, string playerName) {

        EventManager.Instance.triggerEvent("startPlayerTurn/" + playerId, new StartTurnArgs() { playerId = playerId });
        EventManager.Instance.triggerEvent("startPlayerTurn", new StartTurnArgs() { playerId = playerId });
        EventManager.Instance.triggerEvent("web/startTurn", new WebStartTurnArgs() { playerName = playerName, messageTargets = "all" });
    }

}
