using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerOverallData
{
    public List<PlayerMovesData> playerMovesDataLog;
    public int[] attemptsPerLevel;
    public int[] finalStars;
    public int numDragsTotal;
    public int numDragsToDrawer;
    public int numDragsFromDrawer;
    public int numDragsSwitchedSign;

    public PlayerOverallData()
    {
        playerMovesDataLog = new List<PlayerMovesData>();
        attemptsPerLevel = new int[25];
        finalStars = new int[25];
        numDragsTotal = 0;
        numDragsToDrawer = 0;
        numDragsFromDrawer = 0;
        numDragsSwitchedSign = 0;
    }

    public void NewRoundData(PlayerMovesData movesData)
    {
        playerMovesDataLog.Add(movesData);
        int level = movesData.level;
        attemptsPerLevel[level - 1] = attemptsPerLevel[level - 1] + 1;
        if (finalStars[level - 1] < movesData.starsObtained)
        {
            finalStars[level - 1] = movesData.starsObtained;
        }
        numDragsTotal = numDragsTotal + movesData.numDrags;

        foreach (string dragData in movesData.dragLog)
        {
            string[] check = dragData.Split(new string[] { " from " }, StringSplitOptions.None);
            if (check.Length > 1)
            {
                string[] check2 = check[1].Split(new string[] { " to " }, StringSplitOptions.None);
                string origin = check2[0];
                string dest = check2[1];
                
                if (origin.Contains("Drawer"))
                {
                    numDragsFromDrawer++;
                }
                if (dest.Contains("Drawer"))
                {
                    numDragsToDrawer++;
                }
                
                if ((origin.StartsWith("L") && dest.StartsWith("R")) || (origin.StartsWith("R") && dest.StartsWith("L")))
                {
                    if ((origin.EndsWith("Negative") && dest.EndsWith("Positive")) || (origin.EndsWith("Positive") && dest.EndsWith("Negative")))
                    {
                        numDragsSwitchedSign++;
                    }
                }
            }
        }
        
    }
}
