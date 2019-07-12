using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueModuleManager : MonoBehaviour
{
    public Text speakerText;
    public Text dialogueText;
    public TutorialController tutController;
    public EndingSceneController endController;
    public GameObject continueButton;

    private Queue<string> dialogueQueue;
    private string speaker;
    private SoundEffectManager soundEffects;
    private Animator anim;
    private int batch;
    private bool firstDialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
        tutController = FindObjectOfType<TutorialController>();
        endController = FindObjectOfType<EndingSceneController>();
        soundEffects = FindObjectOfType<SoundEffectManager>();
        continueButton.SetActive(true);
        firstDialogue = false;
        batch = 1;
    }

    public int GetBatch()
    {
        return batch;
    }

    public void SetBatch(int newbatch)
    {
        batch = newbatch;
    }

    public void InitDialogue(string speaker, string[] dialogueSentences) {
        continueButton.SetActive(true);
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        StartCoroutine(openDialogueBox(speaker, true));
    }

    public void ContinueDialogue(string speaker, string[] dialogueSentences) {
        continueButton.SetActive(true);
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void ExitDialogueBox()
    {
        anim.SetTrigger("moveOffscreen");
    }

    IEnumerator openDialogueBox(string speaker, bool moveBox) {

        if (moveBox)
        {
            anim.SetTrigger("moveOnscreen");
            yield return new WaitForSeconds(2f);
        }

        StartCoroutine(startSpeaking(speaker));
    }

    public void DisplayNextSentence() {

        if (dialogueQueue.Count == 0) {
            return;
        }

        if (! firstDialogue)
        {
            soundEffects.PlayClicked();
        }

        // Debug.Log(dialogueQueue.Count);
        if (tutController != null)
        {
            tutController.CurrentDialogue(dialogueQueue.Count);
        }
        
        firstDialogue = false;
        string nextSentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(nextSentence));

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
    }

    IEnumerator TypeDialogue(string inputString) {
        dialogueText.text = "";
        foreach (char letter in inputString) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    IEnumerator startSpeaking(string inputString) {
        speakerText.text = "";
        foreach (char letter in inputString) {
            speakerText.text += letter;
            yield return new WaitForSeconds(0.2f);
        }
        firstDialogue = true;
        anim.SetTrigger("enableContinue");
        DisplayNextSentence();
    }

    void EndDialogue() {
        continueButton.SetActive(false);

        if (tutController != null)
        {
            if (batch == 1)
            {
                batch = 2;
                tutController.FinishedFirstDialogue();
            }
            else if (batch == 2)
            {
                batch = 3;
                tutController.FinishedSecondDialogue();
            }
            else if (batch == 3)
            {
                batch = 4;
                tutController.FinishedThirdDialogue();
            }
            else if (batch == 4)
            {
                batch = 5;
                tutController.FinishedFourthDialogue();
            }
        }
        if (endController != null)
        {            
            Debug.Log("Here");
            endController.FinishedDialogue();
        }
    }
}
