using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store information about the player
/// </summary>
public class PlayerInfo : MonoBehaviour {

    private static int NumPlayers = 0;
    private string _playerId;

    public string playerName;

    /// <summary>
    /// Player's unique id
    /// </summary>
	public string playerId {
        get {
            return _playerId;
        }

        set {
            _playerId = value;
        }
    }

    public string websocketId {
        get {
            return playerId;
        }

        set {
            playerId = value;
        }
    }
}
