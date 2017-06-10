using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tween))]
public class CardInteractionHandler : MonoBehaviour {

    public Tween tween;
    public ICardCompleteDraw cardCompleteDraw;

    private void Start() {

        if(tween == null)
            tween = GetComponent<Tween>();

        if(cardCompleteDraw == null)
            cardCompleteDraw = GetComponent<ICardCompleteDraw>();
    }

    public void onDraw(int playerID) {
        
        //begin draw animation (move card to top of screen)
        tween.startTween(transform.position.y, transform.position.y + 15, 0.5f,
            newVal => transform.position = new Vector3(transform.position.x, newVal, transform.position.z),
            () => { cardCompleteDraw.onDrawAnimationComplete(playerID); });
    }
}
