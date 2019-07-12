using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    public TutorialManager tutManager;
    public DialogueTrigger dialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogue.TriggerInitialDialogue();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void FinishedDialogue()
    {
        StartCoroutine(tutManager.EndDialogue());
    }
}
