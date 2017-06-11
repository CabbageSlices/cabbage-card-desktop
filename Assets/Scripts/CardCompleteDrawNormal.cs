using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagement;

/// <summary>
/// Script for cards that should complete their draws normally and send the card to the player who drew it
/// </summary>
public class CardCompleteDrawNormal : MonoBehaviour, ICardCompleteDraw {

	public void onDrawAnimationComplete(int playerID) {
        EventManager.Instance.triggerEvent("DrawCard/done/" + playerID, new  DrawCardResponseArgs() {card = gameObject});
    }
}
