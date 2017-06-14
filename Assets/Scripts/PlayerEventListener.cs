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
        EventManager.Instance.registerCallbackForEvent("drawcard/done/" + playerInfo.playerId, onDrawResponse);
        EventManager.Instance.registerCallbackForEvent("useCardEffect/done/" + playerInfo.playerId, onCardEffectDone);
        EventManager.Instance.registerCallbackForEvent("useCardEffect/" + playerInfo.playerId, onUseCardEffect);
    }

    public void onDrawResponse(EventArgs e) {

        DrawCardResponseArgs args = (DrawCardResponseArgs)e;
        playerHand.addCard(args.card);

        //end player turn
        EventManager.Instance.triggerEvent("endPlayerTurn", new EndTurnArgs() { playerId = playerInfo.playerId });
    }

    public void onCardEffectDone(EventArgs e) {
        //not implemented
        Debug.Log("Card Effect Finished");
    }

    public void onUseCardEffect(EventArgs e) {
        CardEffectUseArgs args = (CardEffectUseArgs)e;
        GameObject card = playerHand.removeCard(args.cardID);

        if(card == null) {
            Debug.Log("Player tried using a card he doesnt have");
            return;
        }

        card.GetComponent<CardInteractionHandler>().onActivateEffect(playerInfo.playerId);
    }
}
