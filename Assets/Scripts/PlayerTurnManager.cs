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
        
    }

    public void startGame() {
        indexCurrentPlayer = 0;
        indexNextPlayer = calculateIndexNextPlayer();
    }

    /// <summary>
    /// setup player id's so that the current player is the next player in line, and calculates a new next player
    /// returns the id of the player whose  turn should start
    /// </summary>
    /// <returns></returns>
    public string startNextPlayerTurn() {

        indexCurrentPlayer = indexNextPlayer;
        indexNextPlayer = calculateIndexNextPlayer();

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

    /// <summary>
    /// Marks the given player as dead so that he his turn doesn't come up again
    /// If it is currently his turn then he must end his turn (allows them to do other stuff once they die)
    /// </summary>
    /// <param name="playerId">id of player who died</param>
    public void onPlayerDeath(string playerId) {

        Transform toRemove = null;
        foreach (Transform child in transform) {

            if (child.gameObject.GetComponent<PlayerInfo>().playerId == playerId) {
                toRemove = child;
                break;
            }
        }
        
        if(toRemove != null && toRemove.GetSiblingIndex() == indexNextPlayer)
            indexNextPlayer = calculateIndexNextPlayer();
    }

    private int calculateIndexNextPlayer() {
        
        int next = indexCurrentPlayer;

        //make sure chosen player isn't dead
        do {
            next = turnDirection == TurnDirection.Clockwise ? next + 1 : next - 1;

            if (next < 0)
                next = transform.childCount - 1;

            next = next % transform.childCount;

        } while (!transform.GetChild(next).GetComponent<PlayerStatus>().isAlive && next != indexCurrentPlayer);//if next == indexCurrentPlayyer then we looped over everyone but theyr'e all dead so stop

        return next;
    }

    
}
