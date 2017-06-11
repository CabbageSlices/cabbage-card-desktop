using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used by all scripts that provide an effect for cards
/// draw completion effect is any effect that should be activated when the card's draw animation is finished
/// </summary>
public interface ICardActivateEffect {

    /// <summary>
    /// Function to call when the card's effect is activated
    /// </summary>
    /// <param name="playerId">id of the player who activated the card effect</param>
	void onTriggerEffect(int playerId);
}
