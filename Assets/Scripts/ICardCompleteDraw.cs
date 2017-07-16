using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used by all scripts that provide a draw completion effect for cards
/// draw completion effect is any effect that should be activated when the card's draw animation is finished
/// </summary>
public interface ICardCompleteDraw {

    /// <summary>
    /// Function to call when the card's drawing animation completes.
    /// </summary>
    /// <param name="playerId">id of the player who drew the card</param>
	void onDrawAnimationComplete(string playerId);
}
