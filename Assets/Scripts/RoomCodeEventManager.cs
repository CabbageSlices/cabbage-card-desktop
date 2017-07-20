using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

[RequireComponent(typeof(RoomCodeDisplay))]
public class RoomCodeEventManager : MonoBehaviour {

    RoomCodeDisplay roomCode;

    // Use this for initialization
    private void Start() {
        if(roomCode == null)
            roomCode = GetComponent<RoomCodeDisplay>();

        subscribeToEvents();
    }

    void subscribeToEvents() {
        EventManager.Instance.registerCallbackForEvent("setRoomCode", setRoomCode);
    }

    public void setRoomCode(EventArgs e) {

        SetRoomCodeArgs args = (SetRoomCodeArgs)e;
        roomCode.setRoomCode(args.roomCode);
    }
}
