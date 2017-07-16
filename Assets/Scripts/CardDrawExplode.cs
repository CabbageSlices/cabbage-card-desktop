using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

/// <summary>
/// Card that will explode when drawn. Informs the player that he exploded
/// </summary>
public class CardDrawExplode : MonoBehaviour, ICardCompleteDraw {

    public void onDrawAnimationComplete(string playerID) {
        EventManager.Instance.triggerEvent("DrawCard/explosion/" + playerID, new DrawCardResponseArgs() { card = gameObject });
    }
}