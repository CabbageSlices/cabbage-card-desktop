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
[RequireComponent(typeof(PlayerStatus))]
public class PlayerEventManager : MonoBehaviour {

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
        EventManager.Instance.registerCallbackForEvent("drawcard/done/" + playerInfo.playerId, onDrawSafeResponse);
        EventManager.Instance.registerCallbackForEvent("useCardEffect/done/" + playerInfo.playerId, onCardEffectDone);
        EventManager.Instance.registerCallbackForEvent("useCardEffect/" + playerInfo.playerId, onUseCardEffect);
        EventManager.Instance.registerCallbackForEvent("drawcard/explosion/" + playerInfo.playerId, onDrawExplosionResponse);
        EventManager.Instance.registerCallbackForEvent("discardForRandomDraw/" + playerInfo.playerId, onDiscardForRandomDraw);
        EventManager.Instance.registerCallbackForEvent("giveupRandomCard/" + playerInfo.playerId, onGiveupRandomCard);
        EventManager.Instance.registerCallbackForEvent("receiveCard/" + playerInfo.playerId, onReceiveCard);
        EventManager.Instance.registerCallbackForEvent("getStartingHand/done/" + playerInfo.playerId, onGetStartingHandDone);
        EventManager.Instance.registerCallbackForEvent("notifyGameStarted", onNotifyGameStarted);
        EventManager.Instance.registerCallbackForEvent("drawStartingHand", onDrawStartingHand);
    }

    public void onDrawSafeResponse(EventArgs e) {

        DrawCardResponseArgs args = (DrawCardResponseArgs)e;
        playerHand.addCard(args.card);

        //end player turn
        EventManager.Instance.triggerEvent("endPlayerTurn", new EndTurnArgs() { playerId = playerInfo.playerId });
    }

    public void onDrawExplosionResponse(EventArgs e) {

        //something about diffuse

        //if player has no diffuse
        //kill self
        GetComponent<PlayerStatus>().isAlive = false;
        EventManager.Instance.triggerEvent("playerDeath", new PlayerDeathArgs() { playerId = playerInfo.playerId });
        EventManager.Instance.triggerEvent("endPlayerTurn", new EndTurnArgs() { playerId = playerInfo.playerId });
    }

    public void onCardEffectDone(EventArgs e) {
        //not implemented
        Debug.Log("Card Effect Finished");
    }

    public void onUseCardEffect(EventArgs e) {
        CardEffectUseArgs args = (CardEffectUseArgs)e;
        GameObject card = playerHand.removeCard(args.cardID);

        if (card == null) {
            Debug.Log("Player tried using a card he doesnt have");
            return;
        }

        card.GetComponent<CardInteractionHandler>().onActivateEffect(playerInfo.playerId);
    }

    public void onDiscardForRandomDraw(EventArgs e) {
        //SelectCardsArgs args = (SelectCardsArgs)e;
        //List<GameObject> cards = playerHand.removeCards(args.cards);

        CardRandomDrawEffect randomDraw = gameObject.AddComponent<CardRandomDrawEffect>();
        randomDraw.onComplete = () => Destroy(randomDraw);
        randomDraw.onTriggerEffect(playerInfo.playerId);
    }

    public void onGiveupRandomCard(EventArgs e) {

        GameObject card = playerHand.removeRandomCard();
        EventManager.Instance.triggerEvent("giveupRandomCard/done/" + playerInfo.playerId, new GiveupRandomCardResponseArgs() { card = card });
    }

    public void onReceiveCard(EventArgs e) {
        ReceiveCardArgs args = (ReceiveCardArgs)e;
        playerHand.addCard(args.card);
    }

    public void onDrawStartingHand(EventArgs e) {
        DrawStartingHandArgs args = (DrawStartingHandArgs)e;

        Debug.Log("Drawing");
        EventManager.Instance.triggerEvent("getStartingHand", new GetStartingHandArgs { numCards = args.numCards, playerId = playerInfo.playerId });
    }

    public void onGetStartingHandDone(EventArgs e) {
        GetStartingHandResponseArgs args = (GetStartingHandResponseArgs)e;
        Debug.Log("saving starting hand");
        playerHand.addCards(args.cards);
    }

    public void onNotifyGameStarted(EventArgs e) {

        WebGameStartArgs args = new WebGameStartArgs { cards = playerHand.cardIds, messageTarget = playerInfo.playerId};

        EventManager.Instance.triggerEvent("web/gameStart", args);
    }
}
