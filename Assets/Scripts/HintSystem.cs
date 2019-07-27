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
    public GameController gameController;

    // private string objective = "Isolate one box on one side with only toys on the other.";
    // private string informhint;
    private string overflow = "Combine or move some terms to make more room!";
    private string switchSign = "Switch the sign when dragging from one side to the other.";
    private string isolateVariables = "Try moving all boxes to one side.";
    private string isolateValues = "Try moving all toys to one side.";
    private string combineVariables = "Try combining boxes on the same side by dragging them together.";
    private string combineValues = "Try combining toys on the same side by dragging them together.";
    private string expandBrackets = "Try expanding the brackets.";
    private string boxPositive = "The box needs to be positive.";
    private string boxOne = "Make sure box's coefficient is one.";
    private string pressDone = "Press Done when you're finished isolating the box.";
    private System.Random random;
    private Coroutine currentHint;
    private List<string> dragLog;
    private int numBrackets;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        dragLog = new List<string>();
        numBrackets = gameController.GetInitialBrackets();
        level = gameController.GetLevel();
        random = new System.Random();
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
            else if (origin2.StartsWith("R") && dest2.StartsWith("R") && ((origin2.EndsWith("Positive") && dest2.EndsWith("Negative")) || (origin2.EndsWith("Negative") && dest2.EndsWith("Positive"))))
            {
                // if moving an item to the same side different sign
                if (hintText.text != switchSign)
                {
                    StartCoroutine(ShowHint(switchSign));
                }
            }
            else if (origin2.StartsWith("L") && dest2.StartsWith("L") && ((origin2.EndsWith("Positive") && dest2.EndsWith("Negative")) || (origin2.EndsWith("Negative") && dest2.EndsWith("Positive"))))
            {
                if (hintText.text != switchSign)
                {
                    StartCoroutine(ShowHint(switchSign));
                }
            }
            else if (origin1 == dest1 && origin2 == dest2 && origin1 != "" && origin2 != "" && dest1 != "" && dest2 != "")
            {
                // player maybe out of ideas, suggest a hint based on seesaw
                InactiveForWhile();
            }
            else if (prevMove == currMove)
            {
                // they made the same move twice in a row, might as well suggest something just in case
                InactiveForWhile();
            }
            // TODO: add if we can think of any other mistakes
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
        else if (seesaw.NumNegativeVariables() > 0)
        {
            StartCoroutine(ShowHint(boxPositive));
        }
        else if (seesaw.CoefficientVariables() != 1)
        {
            StartCoroutine(ShowHint(boxOne));
        }
        else if (seesaw.CheckIfComplete())
        {
            StartCoroutine(ShowHint(pressDone));
        }
        // TODO: add if we can think of any other hints
    }

    public string GetProblemArea()
    {
        // TODO: calculate what's the biggest problem area in the case of them solving the problem correctly
        int wrongDrags = 0;
        int bracketDrags = 0;
        for (int i = 0; i < dragLog.Count; i++)
        {
            if (DraggingError(dragLog[i]))
            {
                wrongDrags = wrongDrags + 1;
            }
            if (BracketDrag(dragLog[i]))
            {
                bracketDrags = bracketDrags + 1;
            }
        }

        if (wrongDrags > 1 && level > 1)
        {
            // a few dragging errors
            return "moving from one side to the other";
        }
        else if (bracketDrags > numBrackets * 2)
        {
            // brackets not expanding enough
            return "expanding brackets";
        }
        else if (random.Next(1, 4) == 1 && level > 3) // TODO: dunno what to check for coefficient review so its just random
        {
            return "coefficients";
        }

        return "";
    }

    public bool DraggingError(string currMove)
    {
        string origin = "";
        string dest = "";
        
        string[] check = currMove.Split(new string[] { " from " }, StringSplitOptions.None);
        if (check.Length > 1)
        {
            string[] check2 = check[1].Split(new string[] { " to " }, StringSplitOptions.None);
            origin = check2[0];
            dest = check2[1];
        }

        // dragged from one side to the other without switching sign
        if (origin.StartsWith("R") && dest.StartsWith("L") && ((origin.EndsWith("Negative") && dest.EndsWith("Negative")) || (origin.EndsWith("Positive") && dest.EndsWith("Positive"))))
        {
            return true;
        }
        else if (origin.StartsWith("L") && dest.StartsWith("R") && ((origin.EndsWith("Negative") && dest.EndsWith("Negative")) || (origin.EndsWith("Positive") && dest.EndsWith("Positive"))))
        {
            return true;
        }
        else if (origin.StartsWith("R") && dest.StartsWith("R") && ((origin.EndsWith("Positive") && dest.EndsWith("Negative")) || (origin.EndsWith("Negative") && dest.EndsWith("Positive"))))
        {
            return true;
        }
        else if (origin.StartsWith("L") && dest.StartsWith("L") && ((origin.EndsWith("Positive") && dest.EndsWith("Negative")) || (origin.EndsWith("Negative") && dest.EndsWith("Positive"))))
        {
            return true;
        }
        return false;
    }

    public bool BracketDrag(string dragData)
    {
        return dragData.StartsWith("Bracket");
    }
}
