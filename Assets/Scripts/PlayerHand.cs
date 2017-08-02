using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Define bevhariour a player's hand
/// allow adding/removing cards, using cards, and more
/// Each card is added as a child to the game object containing this script
/// </summary>
public class PlayerHand : MonoBehaviour {

    public List<GameObject> cards {
        get {

            List<GameObject> cards = new List<GameObject>();
            foreach (Transform card in transform) {

                cards.Add(card.gameObject);
            }

            return cards;
        }
    }

    public List<string> cardIds {
        get {

            List<GameObject> cardObjects = cards;
            
            return (from card in cardObjects
            select card.GetComponent<CardInfo>().cardId).ToList();
        }
    }

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

    public void addCards(List<GameObject> cards) {

        foreach(var card in cards) {
            addCard(card);
        }
    }

    /// <summary>
    /// Remove the card with the given ID from the player's hand. returns the removed  card, or Null if the player doesn't have the card
    /// </summary>
    /// <param name="index">ID of the card to remove</param>
    /// <returns>The card that was removed from the hand. Null if the card doesn't exist</returns>
    public GameObject removeCard(string cardId) {
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
    /// Remove the cards with the given ID from the player's hand. returns all the removed cards, or an empty list if the player ddoesn't have those acards on hand
    /// </summary>
    /// <param name="cardIds">array of ids to remove</param>
    /// <returns></returns>
    public List<GameObject> removeCards(string[] cardIds) {
        
        List<GameObject> removed = new List<GameObject>();
        foreach (Transform card in transform) {

            if (cardIds.Contains(card.gameObject.GetComponent<CardInfo>().cardId)) {
                removed.Add(card.gameObject);
                card.parent = null;

                //enable card so it can do stuff
                card.gameObject.SetActive(true);
            }
        }

        return removed;
    }

    public GameObject removeRandomCard() {
        Transform cardTransform = transform.GetChild(Random.Range(0, transform.childCount));
        cardTransform.parent = null;

        return cardTransform.gameObject;
    }

    /// <summary>
    /// Remove the card with the given name from the player's hand. returns the removed  card, or Null if the player doesn't have the card
    /// </summary>
    /// <param name="cardName">name of the card to remove</param>
    /// <returns>The card that was removed from the hand. Null if the card doesn't exist</returns>
    public GameObject removeCardById(string cardName) {
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
