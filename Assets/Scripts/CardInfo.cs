using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store information about the card, used to identify the card and figure out what pictures to use and so forth
/// </summary>
public class CardInfo : MonoBehaviour {

    ///<Remark>count number of cardInfos created so that each card will have a unique ID </Remark>
    private static int numberOfCardsGenerated = 0;
    public int _cardId;

    ///<Remark>name of  card, for now it should be the same as the card's front image name</Remark>
	public string cardName;

    public int cardId {
        get {
            return _cardId;
        }
    }

    private void Start() {
        _cardId = numberOfCardsGenerated++;
    }
}
