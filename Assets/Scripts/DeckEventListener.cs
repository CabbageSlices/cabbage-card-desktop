using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

/// <summary>
/// Class that listens to events and makes the deck do respond to the events
/// </summary>
[RequireComponent(typeof(Deck))]
public class DeckEventListener : MonoBehaviour {

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
    }
	
	public void onShuffleDeck(EventArgs e) {

        deck.shuffle();
    }

    public void onDrawCard(EventArgs e) {

        DrawCardArgs args = (DrawCardArgs)e;
        GameObject card = deck.drawCard(args.position);
        card.GetComponent<CardInteractionHandler>().onDraw(args.playerID);
        //evaluate card
        //send to game controller to send to player (or send directly)
        //OR allow card to determine how it handles a draw
    }
}

public class DrawCardArgs : EventArgs {

    public int position;
    public int playerID;
}