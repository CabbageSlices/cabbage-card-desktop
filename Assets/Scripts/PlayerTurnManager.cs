using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that will keep track of who's turn it is, who goes next, and notifies player's when their turn starts
/// </summary>
public class PlayerTurnManager : MonoBehaviour {

    /// <summary>
    /// What direction the player's take their turns in.
    /// </summary>
	public enum TurnDirection {
        Clockwise,
        CounterClockwise
    }

    public TurnDirection turnDirection;

    //child index of the current and next player
    public int indexCurrentPlayer;
    private int indexNextPlayer;

    private void Start() {

        indexCurrentPlayer = 0;
        indexNextPlayer = calculateIdNextPlayer();
    }

    /// <summary>
    /// setup player id's so that the current player is the next player in line, and calculates a new next player
    /// returns the id of the player whose  turn should start
    /// </summary>
    /// <returns></returns>
    public int startNextPlayerTurn() {

        indexCurrentPlayer = indexNextPlayer;
        indexNextPlayer = calculateIdNextPlayer();

        return transform.GetChild(indexCurrentPlayer).gameObject.GetComponent<PlayerInfo>().playerId;
    }

    public void reverseTurnDirection() {
        turnDirection = turnDirection == TurnDirection.Clockwise ? TurnDirection.CounterClockwise : TurnDirection.Clockwise;
    }

    /// <summary>
    /// makes given player start his turn
    /// indexNextPlayer will be recalculated using turn direction
    /// </summary>
    /// <param name="nameOfPlayer"></param>
    public void jumpToPlayer(string nameOfPlayer) {

    }

    public void setNextPlayer(string nameOfPlayer) {
        
        foreach(Transform child in transform) {

            if(child.gameObject.GetComponent<PlayerInfo>().playerName == nameOfPlayer) {
                indexNextPlayer = child.GetSiblingIndex();
                break;
            }
        }
        
    }

    private int calculateIdNextPlayer() {
        int next = turnDirection == TurnDirection.Clockwise ? indexCurrentPlayer + 1 : indexCurrentPlayer - 1;

        if(next < 0)
            next = transform.childCount - 1;

        next = next % transform.childCount;
        return next;
    }

    
}
