using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides gameplay functionality for card objects
/// </summary>
[RequireComponent(typeof(Tween))]
public class CardInteractionHandler : MonoBehaviour {

    public Tween tween;
    public ICardCompleteDraw cardCompleteDraw;
    public ICardActivateEffect cardEffect;

    private void Start() {

        if(tween == null)
            tween = GetComponent<Tween>();

        if(cardCompleteDraw == null)
            cardCompleteDraw = GetComponent<ICardCompleteDraw>();

        if(cardEffect == null)
            cardEffect = GetComponent<ICardActivateEffect>();
    }

    public void onDraw(string playerID) {
        
        //begin draw animation (move card to top of screen)
        tween.startTween(transform.position.y, transform.position.y + 15, 0.5f,
            newVal => transform.position = new Vector3(transform.position.x, newVal, transform.position.z),
            () => { cardCompleteDraw.onDrawAnimationComplete(playerID); });
    }

    public void onActivateEffect(string playerId) {
        
        //card has no effect to activate
        if(cardEffect == null) {
            Debug.LogWarning("Player activated card effect for a card that has no effect.");
            return;
        }

        cardEffect.onTriggerEffect(playerId);
    }
}
