using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a deck of cards and provides functions that you'd execpt a deck of cards to be able to do
/// Cards are child gameobjects of the deck, and must be accessed through the deck's transform
/// </summary>
public class Deck : MonoBehaviour {

    public delegate void ActionCompleteCallback();

    public List<GameObject> cardPrefabs;

    public List<GameObject> cards {
        get {
            List<GameObject> cards = new List<GameObject>();

            foreach(Transform child in transform)
                cards.Add(child.gameObject);

            return cards;
        }
    }

	// Use this for initialization
	void Start () {
		
        //don't draw any of the cards except top
        renderNoCards();
        
        renderCard(0);
    }
	
	public void renderAllCards() {
        foreach(Transform child in transform)
            child.gameObject.SetActive(true);
    }

    public void renderNoCards() {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    /// <summary>
    /// Renders the card at the given position. Position must be a 0 based index.
    /// If position is negative then the ecard is indexed starting from the bottom of the deck, with -1 being the card at the very bottom
    /// If position is bigger than the number of cards in the deck then nothing is drawn
    /// </summary>
    /// <param name="position">index of card to draw</param>
    public void renderCard(int position) {

        if (position >= transform.childCount || position < -transform.childCount)
            return;

        if (position < 0)
            position = transform.childCount + position;

        if (transform.GetChild(position) != null)
            transform.GetChild(position).gameObject.SetActive(true);
    }
    
    public void shuffle(bool shouldPlayAnimation, ActionCompleteCallback callback = null) {
        
        for(int child = 0; child < transform.childCount - 1; ++child) {
            
            int swapPosition = Random.Range(child + 1, transform.childCount - 1);

            Transform toSwapWith = transform.GetChild(swapPosition);
            transform.GetChild(child).SetSiblingIndex(swapPosition);
            toSwapWith.SetSiblingIndex(child);
        }

        renderNoCards();
        renderCard(0);

        //if(shouldPlayAnimation)
            //play animation here

        //wwhen shuffle animation completes
        //callback();
    }

    /// <summary>
    /// Draws the card at the given position. This will return the drawn card, and remove the card as a child of the deck
    /// Position should be a 0 based index from the top of the deck, with 0 representing the top of the deck
    /// If a negative position is given then a card is drawn from the bottom of the deck, with -1 being the card at the bottom of the deck
    /// a position that is out of range will return an empty game object
    /// </summary>
    /// <param name="position">Index of the card within the deck</param>
    /// <returns>Reference to the card's gameobject. returns Null if given index is out of range</returns>
    public GameObject drawCard(int position) {
        if(position >= transform.childCount || position < -transform.childCount)
            return null;

        if(position < 0)
            position = transform.childCount + position;

        //if we drew the top card then enable the next card incase it was disabled
        if(position == 0 && position + 1 < transform.childCount)
            transform.GetChild(position + 1).gameObject.SetActive(true);
            

        Transform card = transform.GetChild(position);
        card.parent = null;
        card.gameObject.SetActive(true);
        
        if(position == 0)
            card.GetComponent<CardDisplay>().drawOnTopOfDeck();
        else
            card.GetComponent<CardDisplay>().drawOnBottomOfDeck();

        return card.gameObject;
    }
    
    /// <summary>
    /// Returns 'count' consequitive cards starting from the top of the deck going downards
    /// When any of the cards are out of range, only the cards within range are returned
    /// </summary>
    /// <param name="count">Number of consequitive cards to view</param>
    /// <returns></returns>
    public List<GameObject> viewCardsFromTop(int count) {

        List<GameObject> cards = new List<GameObject>();

        for(int i = 0; i < count && i < transform.childCount; ++i)
            cards.Add(transform.GetChild(i).gameObject);

        activateCards(cards);
        return cards;
    }

    /// <summary>
    /// Returns 'count' consequitive cards starting from the bottom of the deck going upwards
    /// When any of the cards are out of range, only the cards within range are returned
    /// </summary>
    /// <param name="count">Number of consequitive cards to view</param>
    /// <returns></returns>
    public List<GameObject> viewCardsFromBottom(int count) {

        List<GameObject> cards = new List<GameObject>();

        for (int i = transform.childCount - 1; i >= 0 && i >= transform.childCount - count; --i)
            cards.Add(transform.GetChild(i).gameObject);

        activateCards(cards);
        return cards;
    }

    public List<GameObject> drawSafeCards(int count) {

        List<GameObject> cards = new List<GameObject>();

        for(int i = 0; i < transform.childCount && i < count;) {

            var child = transform.GetChild(i).gameObject;
            var info = child.GetComponent<CardInfo>();

            if(info.type == CardInfo.CardType.BOMB) {
                ++i;
                continue;
            }

            cards.Add(child);
            child.transform.SetParent(null);
        }

        return cards;
    }

    public void generate() {
        
        foreach(var prefab in cardPrefabs) {

            for(int i = 0; i < 5; ++i) {
                GameObject card = GameObject.Instantiate(prefab);
                card.transform.SetParent(transform);
                card.transform.localPosition = new Vector3();
            }
        }
    }

    public void insertCards(int position, List<string> cardNames) {

    }
    
    public void insertCards(int position, List<GameObject> cards) {

    }

    public void insertCard(int position, GameObject card) {

    }

    private void activateCards(List<GameObject> cards) {

        foreach(GameObject card in cards)
            card.SetActive(true);
    }
}
