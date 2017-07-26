using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public string playerId;
}

public class StartTurnArgs : System.EventArgs {
    public string playerId;
}

public class PlayerDeathArgs : System.EventArgs {
    public string playerId;
}

public class SelectCardsArgs : System.EventArgs {
    public int[] cards;///<Remark>list of ids of the selected by the player</Remark>
}

public class SelectTargetPlayerArgs: System.EventArgs {
    public string playerInitiatingTarget;
}

public class SelectTargetPlayerResponseArgs : System.EventArgs {
    public string playerBeingTargeted;
}

public class GiveupRandomCardResponseArgs : System.EventArgs {
    public GameObject card;
}

public class ReceiveCardArgs : System.EventArgs {
    public GameObject card;
}

public class DrawCardArgs : EventArgs {

    public int position;
    public string playerId;
}

public class DeckShuffleArgs : EventArgs {
    public string playerId;
}

public class NetworkErrorArgs : EventArgs {
    public string errorMesssage;
}

public class ConnectionClosedArgs : EventArgs {
    public string message;
}

public class GenerateRoomCodeArgs : EventArgs {
    public string roomCode;
}

public class SetRoomCodeArgs : EventArgs {
    public string roomCode;
}

public class ConnectToServerArgs : EventArgs {
    public string playerName;
    public string webClientSocketId;
}

public class ConnectToServerAcceptArgs : EventArgs {
    public string playerName;
    public string webClientSocketId;
    public List<string> connectedPlayers;
}

public class ConnectToServerRejectArgs : EventArgs {
    public string playerName;
    public string webClientSocketId;
    public string message;
}

public class WebClientDisconnectArgs : EventArgs {
    public string webClientSocketId;
}

public class WebPlayerConnectedArgs : EventArgs {
    public string playerName;
    public List<string> messageTargets;
}

public class WebPlayerDisconnectedArgs : EventArgs {
    public string playerName;
    public List<string> messageTargets;
}