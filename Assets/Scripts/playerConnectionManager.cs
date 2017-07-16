using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that keeps track of all the  connected players
/// Accepts new connecetions and handles connection requests
/// </summary>
public class PlayerConnectionManager : MonoBehaviour {

	public GameObject connectPlayer(string playerName, string websocketId) {
        //check if the given player exists yet
        foreach(Transform child in transform) {

            PlayerInfo info = child.gameObject.GetComponent<PlayerInfo>();
            if(info.websocketId == websocketId) {
                info.playerName = playerName;
                return info.gameObject;
            }
        }

        GameObject player = PlayerFactory.createPlayer(playerName, websocketId, websocketId);

        player.transform.SetParent(transform);
        player.transform.localPosition = new Vector3(0, 0, 0);

        return player;
    }

    public void disconnectPlayer(string websocketId) {

        foreach (Transform child in transform) {
            PlayerInfo info = child.gameObject.GetComponent<PlayerInfo>();

            if (info.websocketId == websocketId) {
                child.SetParent(null);
                GameObject.Destroy(child.gameObject);
                return;
            }
        }
    }
}
