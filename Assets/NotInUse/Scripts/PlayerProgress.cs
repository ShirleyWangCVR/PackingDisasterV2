using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Stores variables about the player's current progress.
 */
[System.Serializable]
public class PlayerProgress
{
    public int highestScore; // currently only storing the player's highest score.
    // currently high score isn't much use
    // could probably store equations completed and level they're on if we don't do it in the DataController
}
