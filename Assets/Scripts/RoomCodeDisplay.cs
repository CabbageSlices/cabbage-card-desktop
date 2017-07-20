using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCodeDisplay : MonoBehaviour {

    public Text roomCode;
    
	// Use this for initialization
	void Start () {
		
        if(roomCode == null) {
            Transform codeTransform = transform.Find("RoomCodeText");
            roomCode = codeTransform.GetComponent<Text>();
        }
	}
	
	public void setRoomCode(string code) {
        roomCode.text = code;
    }

    public string getRoomCode() {
        return roomCode.text;
    }
}
