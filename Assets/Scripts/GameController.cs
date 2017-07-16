using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	enum State {
        AWAITING_CONNECTION,
        PLAYING
    }

    State state = State.AWAITING_CONNECTION;

    string roomCode;

    public void setRoomCode(string _roomCode) {
        roomCode = _roomCode;
    }

    public bool isAwaitingConnection() {
        return state == State.AWAITING_CONNECTION;
    }
}
