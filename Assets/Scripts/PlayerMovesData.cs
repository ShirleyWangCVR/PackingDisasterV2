using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Data class containing the information about a player's moves per level
 */
[System.Serializable]
public class PlayerMovesData
{
    public int level;
    public float timeTaken; // time taken overall in this level
    public bool completed; // did they beat the level?
    public int starsObtained; 
    public int numDrags;
    public List<string> dragLog; // track the kind of drag each drag was
    public List<string> equationLog; // track every time the equation changed
    public string reasonLost; // if round lost why, empty if won

    public PlayerMovesData(int currLevel)
    {
        level = currLevel;
        timeTaken = 0f;
        completed = false;
        starsObtained = 0;
        numDrags = 0;
        dragLog = new List<string>();
        equationLog = new List<string>();
        reasonLost = "";
    }

    public void SubmitEndRound(float time, bool done, int stars, string reason)
    {
        timeTaken = time;
        completed = done;
        starsObtained = stars;
        reasonLost = reason;
    }

    public void AddDragLog(string dragged)
    {
        dragLog.Add(dragged);
        numDrags = numDrags + 1;
    }

    public void AddEquationLog(string equation)
    {
        equationLog.Add(equation);
    }

}
