using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A basic hint system for the game.
 */

public class HintSystem : MonoBehaviour
{
    public Text hintText;
    public GameObject hintBubble;
    
    private string objective;
    private string informhint;
    private string overflow;
    private string[] hints;
    private int numhints;
    private int currentIndex;
    private System.Random random;
    private Coroutine currentHint;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set hints to be as follows.
        objective = "Isolate one box on one side with only toys on the other.";
        informhint = "Tap me if you ever need a hint on what to do!";
        overflow = "Combine or move some terms to make more room!";
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

        if (level == 3)
        {
            // inform about hint system then show objective
            currentHint = StartCoroutine(InformThenObjective());
        }
        else
        {
            // show objective
            currentHint = StartCoroutine(ShowHint(objective));
        }
    }

    // show overflow message
    public void SeesawSideOverflow()
    {
        StopCoroutine(currentHint);
        currentHint = StartCoroutine(ShowHint(overflow));
    }

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
        hintBubble.SetActive(true);
        hintText.text = informhint;
        Debug.Log("Part 1");

        yield return new WaitForSeconds(4f);

        Debug.Log("Check");
        hintText.text = objective;
        yield return new WaitForSeconds(4f);
        hintBubble.SetActive(false);
    }

    // show a given hint for 4 seconds before disappearing
    public IEnumerator ShowHint(string hint)
    {
        hintBubble.SetActive(true);
        hintText.text = hint;

        yield return new WaitForSeconds(4f);
        hintBubble.SetActive(false);
    }

}
