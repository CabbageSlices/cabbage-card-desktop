using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store information about the card, used to identify the card and figure out what pictures to use and so forth
/// </summary>
public class CardInfo : MonoBehaviour {
    
    public enum CardType {
        BOMB,
        EFFECT,
        NO_EFFECT,
    }

    public string cardId;

    ///<Remark>name of  card, for now it should be the same as the card's front image name</Remark>
	public string cardName;

    //indicates if it's a bomb or not
    public CardType type;
}
