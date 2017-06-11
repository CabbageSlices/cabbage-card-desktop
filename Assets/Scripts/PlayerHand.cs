﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define bevhariour a player's hand
/// allow adding/removing cards, using cards, and more
/// Each card is added as a child to the game object containing this script
/// </summary>
public class PlayerHand : MonoBehaviour {

    /// <summary>
    /// Add the given card to the  player's hand
    /// </summary>
    /// <param name="card">Card to add to the hand</param>
	public void addCard(GameObject card) {
        card.transform.SetParent(transform);
        card.transform.localPosition = new Vector3(0, 0, 0);

        //hide the card so it's not drawn on screen
        card.SetActive(false);
    }

    /// <summary>
    /// Remove the card with the given ID from the player's hand. returns the removed  card, or Null if the player doesn't have the card
    /// </summary>
    /// <param name="index">ID of the card to remove</param>
    /// <returns>The card that was removed from the hand. Null if the card doesn't exist</returns>
    public GameObject removeCard(int cardId) {
        GameObject toRemove = null;
        foreach(Transform card in transform) {

            if(card.gameObject.GetComponent<CardInfo>().cardId == cardId) {
                toRemove = card.gameObject;
                card.parent = null;
                break;
            }
        }

        return toRemove;
    }

    /// <summary>
    /// Remove the card with the given name from the player's hand. returns the removed  card, or Null if the player doesn't have the card
    /// </summary>
    /// <param name="cardName">name of the card to remove</param>
    /// <returns>The card that was removed from the hand. Null if the card doesn't exist</returns>
    public GameObject removeCard(string cardName) {
        GameObject toRemove = null;
        foreach (Transform card in transform) {

            if (card.gameObject.GetComponent<CardInfo>().cardName == cardName) {
                toRemove = card.gameObject;
                card.parent = null;
                break;
            }
        }

        return toRemove;
    }

    public int getCardCount() {
        return transform.childCount;
    }
}