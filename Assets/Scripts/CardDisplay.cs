using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a single card
/// Provides a way to choose a sprite image by name, display different faces of the card
/// </summary>
public class CardDisplay : MonoBehaviour {

    ///<Remark>Name of the card, used for identification, and to get card image</Remark>
    public string cardName;
    
    public GameObject front;
    public GameObject back;
    
	void Start () {

		if(front == null)
            front = transform.Find("front").gameObject;

        if (back == null)
            back = transform.Find("back").gameObject;

        loadFrontImage();
        showBack();
    }

    private void loadFrontImage() {
        if(cardName == null || cardName == "")
            Debug.LogWarning("Card missing card name!");
        
        Sprite newImage = Resources.Load<Sprite>("cards/" + cardName);
        front.GetComponent<SpriteRenderer>().sprite = newImage;
    }
	
    /// <summary>
    /// Displays the front face of the card
    /// </summary>
	public void showFront() {

        front.SetActive(true);
        back.SetActive(false);
    }

    /// <summary>
    /// Displays the back face of the card (default)
    /// </summary>
    public void showBack() {
        
        front.SetActive(false);
        back.SetActive(true);
    }

    public bool isFrontShown() {
        return front.activeSelf;
    }

    public bool isBackShown() {
        return back.activeSelf;
    }
    
    /// <summary>
    /// Force  the card to draw on top of deck
    /// </summary>
    public void drawOnTop() {

        front.GetComponent<SpriteRenderer>().sortingOrder = 3;
        back.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    /// <summary>
    /// Force the card to draw on bottom of deck
    /// </summary>
    public void drawOnBottom() {

        front.GetComponent<SpriteRenderer>().sortingOrder = 0;
        back.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
}
