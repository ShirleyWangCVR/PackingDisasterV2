using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 6)]
    public string[] dialogueSentences;

    public TutorialManager tutManager;

    void Start()
    {
        /* tutManager = FindObjectOfType<TutorialManager>();
        if (tutManager == null)
        {
            Debug.Log("Tutorial Manager not found initially");
        } */
    }

    public void TriggerInitialDialogue() {
        // Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        if (tutManager == null)
        {
            Debug.Log("Tutorial Manager not found");
        }
        if (tutManager == null)
        {
            tutManager.InitBobDialogue(dialogueSentences);
        }

        StartCoroutine(tutManager.InitBobDialogue(dialogueSentences));
    }

    public void TriggerDialogue() {
        // Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        StartCoroutine(tutManager.BobDialogue(dialogueSentences));
    }
}
