using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A basic hint system for the game.
 */

public class HintSystem : MonoBehaviour
{
    public Text hintText;
    public Image hintBubble;
    public SeesawController seesaw;

    private string objective = "Isolate one box on one side with only toys on the other.";
    private string informhint;
    private string overflow = "Combine or move some terms to make more room!";
    private string switchSign = "Switch the sign when dragging from one side to the other.";
    private string isolateVariables = "Try moving all boxes to one side.";
    private string isolateValues = "Try moving all toys to one side.";
    private string combineVariables = "Try combining boxes on the same side by dragging them together.";
    private string combineValues = "Try combining toys on the same side by dragging them together.";
    private string expandBrackets = "Try expanding the brackets.";
    private string[] hints;
    private int numhints;
    private int currentIndex;
    private System.Random random;
    private Coroutine currentHint;
    private List<string> dragLog;

    // Start is called before the first frame update
    void Start()
    {
        /* // Set hints to be as follows.
        // objective = "Isolate one box on one side with only toys on the other.";
        informhint = "Tap me if you ever need a hint on what to do!";
        // overflow = "Combine or move some terms to make more room!";
        hints = new string[] { "Isolate one box on one side with only toys on the other.",
                               "Anything you do to one side you should do to the other.",
                               "Drag an item from one side to the other and switch its sign to keep the seesaw balanced.",
                               "Cancel out a positive and negative on the same side by dragging one on the other.",
                               "The number in front of an object represents how many of them there are.",
                               "Combine two terms of the same kind by dragging one on top of the other.",
                               "Double click a term to split it apart by one.",
                               "Press the equals sign to do an operation to both sides.",
                               "You can't do anything to the terms inside the bracket until you expand it.",
                               "Expand the bracket by dragging its coefficient onto all coefficients inside it." };

        currentIndex = -1;
        random = new System.Random();
        int level = FindObjectOfType<DataController>().GetDifficulty();
        if (level < 6)
        {
            numhints = 4;
        }
        else if (level < 11)
        {
            numhints = 7;
        }
        else if (level < 16)
        {
            numhints = 8;
        }
        else
        {
            numhints = 10;
        }
 */
        /* if (level == 3)
        {
            // inform about hint system then show objective
            currentHint = StartCoroutine(InformThenObjective());
        }
        else
        {
            // show objective
            currentHint = StartCoroutine(ShowHint(objective));
        } */

        currentHint = StartCoroutine(ShowHint(objective));
        dragLog = new List<string>();
    }

    // show overflow message
    public void SeesawSideOverflow()
    {
        StopCoroutine(currentHint);
        currentHint = StartCoroutine(ShowHint(overflow));
    }

    // show a given hint for 4 seconds before disappearing
    public IEnumerator ShowHint(string hint)
    {
        hintBubble.enabled = true;
        hintText.text = hint;

        yield return new WaitForSeconds(4f);
        hintBubble.enabled = false;
        hintText.text = "";
    }

    // when a player has dragged
    public void AddDragInfo(string dragData)
    {
        dragLog.Add(dragData);

        if (dragLog.Count > 1)
        {
            string prevMove = dragLog[dragLog.Count - 2];
            string currMove = dragData;

            string origin1 = "";
            string origin2 = "";
            string dest1 = "";
            string dest2 = "";

            // check last two drags to see if mistake or pointless
            string[] check = prevMove.Split(new string[] { " from " }, StringSplitOptions.None);
            if (check.Length > 1)
            {
                string[] check2 = check[1].Split(new string[] { " to " }, StringSplitOptions.None);
                origin1 = check2[0];
                dest1 = check2[1];
            }

            string[] check3 = currMove.Split(new string[] { " from " }, StringSplitOptions.None);
            if (check3.Length > 1)
            {
                string[] check4 = check3[1].Split(new string[] { " to " }, StringSplitOptions.None);
                origin2 = check4[0];
                dest2 = check4[1];
            }

            // dragged from one side to the other without switching sign
            if (origin2.StartsWith("R") && dest2.StartsWith("L") && ((origin2.EndsWith("Negative") && dest2.EndsWith("Negative")) || (origin2.EndsWith("Positive") && dest2.EndsWith("Positive"))))
            {
                if (hintText.text != switchSign)
                {
                    StartCoroutine(ShowHint(switchSign));
                }
            }
            else if (origin2.StartsWith("L") && dest2.StartsWith("R") && ((origin2.EndsWith("Negative") && dest2.EndsWith("Negative")) || (origin2.EndsWith("Positive") && dest2.EndsWith("Positive"))))
            {
                if (hintText.text != switchSign)
                {
                    StartCoroutine(ShowHint(switchSign));
                }
            }
            else if (origin1 == dest1 && origin2 == dest2)
            {
                // player maybe out of ideas, suggest a hint based on seesaw
                InactiveForWhile();
            }
            // TODO: add if we can think of any other mistakes
            // if moving an item to the same side different sign 

        }
    }

    // checks the current seesaw status and suggest a hint based on what's left
    public void InactiveForWhile()
    {
        // check seesaw status and suggest hint
        if (seesaw.LeftSideNumBrackets() > 0 || seesaw.RightSideNumBrackets() > 0)
        {
            StartCoroutine(ShowHint(expandBrackets));
        }
        else if (seesaw.LeftSideNumVariables() > 0 && seesaw.RightSideNumVariables() > 0)
        {
            StartCoroutine(ShowHint(isolateVariables));
        }
        else if (seesaw.LeftSideNumValues() > 0 && seesaw.RightSideNumValues() > 0)
        {
            StartCoroutine(ShowHint(isolateValues));
        }
        else if (seesaw.LeftSideNumVariables() > 1 || seesaw.RightSideNumVariables() > 1)
        {
            StartCoroutine(ShowHint(combineVariables));
        }
        else if (seesaw.LeftSideNumValues() > 1 || seesaw.RightSideNumValues() > 1)
        {
            StartCoroutine(ShowHint(combineValues));
        }
        // TODO: add if we can think of any other hints
        // remind that box has to be Positive
        // remind that box coefficient needs to be one
    }




















    // probably not in use right now

    // get a random hint different from the previous
    public void GetHint()
    {
        // generate random number for random hint
        int index = random.Next(0, numhints);
        while (index == currentIndex)
        {
            index = random.Next(0, numhints);
        }

        currentIndex = index;
        StopCoroutine(currentHint);
        currentHint = StartCoroutine(ShowHint(hints[index]));
    }

    // explain hint system then show objective
    public IEnumerator InformThenObjective()
    {
        // hintBubble.SetActive(true);
        hintBubble.enabled = true;
        hintText.text = informhint;
        Debug.Log("Part 1");

        yield return new WaitForSeconds(4f);

        Debug.Log("Check");
        hintText.text = objective;
        yield return new WaitForSeconds(4f);
        hintBubble.enabled = false;
        hintText.text = "";
    }
}
