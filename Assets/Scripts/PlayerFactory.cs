using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class PlayerFactory {

	public static GameObject createPlayer(string playerName, string playerId, string websocketId) {
        GameObject playerPrefab = Resources.Load<GameObject>("player");
        GameObject player = GameObject.Instantiate(playerPrefab);

        PlayerInfo info = player.GetComponent<PlayerInfo>();
        info.playerName = playerName;
        info.playerId = playerId;
        info.websocketId = websocketId;

        return player;
    }
}
