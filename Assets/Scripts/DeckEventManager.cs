using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

/// <summary>
/// Class that listens to events and makes the deck respond to the events
/// </summary>
[RequireComponent(typeof(Deck))]
public class DeckEventManager : MonoBehaviour {

    //reference to the deck script
    public Deck deck;
    
	void Start () {

		if(deck == null)
            deck = GetComponent<Deck>();

        subscribeToEvents();
	}

    private void subscribeToEvents() {

        EventManager.Instance.registerCallbackForEvent("ShuffleDeck", onShuffleDeck);
        EventManager.Instance.registerCallbackForEvent("DrawCard", onDrawCard);
        EventManager.Instance.registerCallbackForEvent("getStartingHand", onGetStartingHand);
    }
	
	public void onShuffleDeck(EventArgs e) {
        
        deck.shuffle(true, () => { EventManager.Instance.triggerEvent("ShuffleDeck/done", e); } );
    }

    public void onDrawCard(EventArgs e) {

        DrawCardArgs args = (DrawCardArgs)e;
        GameObject card = deck.drawCard(args.position);
        card.GetComponent<CardInteractionHandler>().onDraw(args.playerId);
        //evaluate card
        //send to game controller to send to player (or send directly)
        //OR allow card to determine how it handles a draw
    }

    public void onGetStartingHand(EventArgs e) {
        GetStartingHandArgs args = (GetStartingHandArgs)e;

        var cards = deck.drawSafeCards(args.numCards);
        
        Debug.Log("returning hand" + cards.Count + "   " + args.numCards);
        EventManager.Instance.triggerEvent("getStartingHand/done/" + args.playerId, new GetStartingHandResponseArgs { cards = cards });
    }
}