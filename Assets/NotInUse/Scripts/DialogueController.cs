/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueController : MonoBehaviour
{
    public DialogueData dialogueData;
    public GameObject dialogueDisplay;
    public Text userText;

    private DataController dataController;
    private bool dialogueActive;
    private string[] currentDialogue;
    // index of the next dialogue to show
    private int dialogueIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        // get dialogue from data controller
        dataController = FindObjectOfType<DataController>();
        dialogueData = dataController.dialogue;
    }

    // start the tutorial dialogue
    public void ExecuteTutorialDialogue()
    {
        dialogueActive = true;
        dialogueDisplay.SetActive(true);
        currentDialogue = dialogueData.level0DialogueList;
        dialogueIndex = 0;
        ShowDialogue();
    }

    // show the next dialogue
    public void ShowDialogue()
    {
        userText.text = currentDialogue[dialogueIndex];
    }

   
    public void OnClick()
    {
        if (dialogueActive)
        {
            dialogueIndex++;
            if (dialogueIndex == currentDialogue.Length)
            {
                // out of dialogue
                dialogueActive = false;
                dialogueDisplay.SetActive(false);
                return;
            }
            ShowDialogue();
        }
    }

    // Are we finished dialogue?
    public bool FinishedDialogue()
    {
        return ! dialogueActive;
    }
}
 */