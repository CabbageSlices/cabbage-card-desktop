using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store information about the player
/// </summary>
public class PlayerInfo : MonoBehaviour {

    private static int NumPlayers = 0;
    private int _playerId;

    public string playerName;

    /// <summary>
    /// Player's unique id
    /// </summary>
	public int playerId {
        get {
            return _playerId;
        }
    }

    private void Start() {
        _playerId = NumPlayers++;
    }
}
