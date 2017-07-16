using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagement;
using System;

/// <summary>
/// Card effect that will shuffle the deck
/// </summary>
public class CardEffectShuffle : MonoBehaviour, ICardActivateEffect {

    public void onTriggerEffect(string playerID) {
        EventManager.Instance.registerCallbackForEvent("shuffleDeck/done", onShuffleComplete);
        EventManager.Instance.triggerEvent("shuffledeck", new DeckShuffleArgs() { playerId = playerID } );
    }

    public void onShuffleComplete(EventArgs e) {
        DeckShuffleArgs args = (DeckShuffleArgs)e;
        EventManager.Instance.triggerEvent("useCardEffect/done/" + args.playerId, e);
    }
}
