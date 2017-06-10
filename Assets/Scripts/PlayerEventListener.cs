using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

/// <summary>
/// Class that listens to events and makes the player respond to events
/// </summary>
[RequireComponent(typeof(PlayerHand))]
[RequireComponent(typeof(PlayerInfo))]
public class PlayerEventListener : MonoBehaviour {

    public PlayerHand playerHand;
    public PlayerInfo playerInfo;

    // Use this for initialization
    void Start() {

        if (playerHand == null)
            playerHand = GetComponent<PlayerHand>();

        if (playerInfo == null)
            playerInfo = GetComponent<PlayerInfo>();

        //delay subscribe to event call so that the player's id will be probably set up (start method call order isn't set)
        Invoke("subscribeToEvents", 0.05f);
    }

    private void subscribeToEvents() {

        //listen for events specific to this player
        EventManager.Instance.registerCallbackForEvent("drawcard/response/" + playerInfo.playerId, onDrawResponse);
    }

    public void onDrawResponse(EventArgs e) {

        DrawCardResponseArgs args = (DrawCardResponseArgs)e;
        playerHand.addCard(args.card);
    }
}
