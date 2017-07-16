using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

public class CardRandomDrawEffect : MonoBehaviour, ICardActivateEffect {

    private string idPlayerActivatingEffect;

    public delegate void OnCompleteCallback();
    
    public OnCompleteCallback onComplete;

    public void onTriggerEffect(string playerId) {
        idPlayerActivatingEffect = playerId;

        EventManager.Instance.registerCallbackForEvent("selectTargetPlayer/done/" + idPlayerActivatingEffect, onSelectTargetDone);

        //notify player that he needs to select a player to draw from
        EventManager.Instance.triggerEvent("selectTargetPlayer", new SelectTargetPlayerArgs() { playerInitiatingTarget = playerId });//send ot front end to select card
    }

    public void onSelectTargetDone(EventArgs e) {
        SelectTargetPlayerResponseArgs args = (SelectTargetPlayerResponseArgs)e;

        EventManager.Instance.registerCallbackForEvent("giveupRandomCard/done/" + args.playerBeingTargeted, onReceiveRandomCard);

        //get random card from targetted player's hand
        EventManager.Instance.triggerEvent("giveupRandomCard/" + args.playerBeingTargeted, new EventArgs());
    }

    public void onReceiveRandomCard(EventArgs e) {
        GiveupRandomCardResponseArgs args = (GiveupRandomCardResponseArgs)e;

        EventManager.Instance.triggerEvent("receiveCard/" + idPlayerActivatingEffect, new ReceiveCardArgs() { card = args.card });
        EventManager.Instance.triggerEvent("useCardEffect/done/" + idPlayerActivatingEffect, new ReceiveCardArgs() { card = args.card });

        if(onComplete != null)
            onComplete();
    }
}
