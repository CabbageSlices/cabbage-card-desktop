using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectedPlayersDisplay : MonoBehaviour {

	public GameObject playerNamePrefab;
    public GameObject playerNamesPanel;

    private void Start() {

        if(playerNamePrefab == null)
            Debug.LogWarning("PlayerNamePrefab is null in connectedPlayersDisplay");

        if(playerNamesPanel == null)
            playerNamesPanel = transform.Find("PlayerNames").gameObject;

        removeAllPlayers();
    }

    private void removeAllPlayers() {

        for (int i = 0; i < playerNamesPanel.transform.childCount;) {

            Transform child = playerNamesPanel.transform.GetChild(i);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void addConnectedPlayer(string playerName, string playerId) {

        GameObject newPlayer = Instantiate(playerNamePrefab, playerNamesPanel.transform);
        newPlayer.GetComponent<Text>().text = playerName;

        PlayerInfo info = newPlayer.GetComponent<PlayerInfo>();
        info.playerName = playerName;
        info.playerId = playerId;
    }

    public void removeConnectedPlayer(string playerId) {
        
        for(int i = 0; i < playerNamesPanel.transform.childCount; ) {
            
            Transform child = playerNamesPanel.transform.GetChild(i);
            PlayerInfo info = child.GetComponent<PlayerInfo>();
            
            if(info.playerId == playerId) {
                child.SetParent(null);
                Destroy(child.gameObject);
                continue;
            } 

            ++i;
        }
    }
}
