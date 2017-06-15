using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// argumetns for response event to draw card event
/// </summary>
public class DrawCardResponseArgs : System.EventArgs {
    public GameObject card; ///<Remark>card that was drawn</Remark>
}

/// <summary>
/// arguments for when player chooses to use a card effect
/// </summary>
public class CardEffectUseArgs : System.EventArgs {
    public int cardID;///<Remark>id of the card to use</Remark>
}

public  class EndTurnArgs : System.EventArgs {
    public int playerId;
}

public class StartTurnArgs : System.EventArgs {
    public int playerId;
}

public class PlayerDeathArgs : System.EventArgs {
    public int playerId;
}