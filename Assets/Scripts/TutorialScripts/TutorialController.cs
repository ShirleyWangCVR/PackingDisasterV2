using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The controller of the interactive elements of the tutorials
public class TutorialController : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public DialogueModuleManager dialogueModuleManager;
    public GameObject interactivePanel;
    public DialogueTrigger[] dialogueTriggers;
    public GameObject seesaw;
    public GameObject skipButton;
    public Transform checkLHSPositive;

    private DataController dataController;
    private int tutorialLevel;
    private int dialogueNum;
    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;
    private bool finishedSecondDrag;
    private bool startingUp;
    private bool doneTutorial;
    private bool checkArrow;
    private bool flash;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        tutorialLevel = dataController.GetDifficulty();

        checkLHSPositive = seesaw.transform.Find("LHSPositive");

        if (dataController.GetTriedTutorial(tutorialLevel) > 0)
        {
            skipButton.SetActive(true);
        }

        dataController.SetTriedTutorial(tutorialLevel, 1);

        dialogueNum = 1;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;
        finishedSecondDrag = false;
        doneTutorial = false;
        checkArrow = false;
        flash = false;

        if (tutorialLevel == 1)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 2)
        {
            dialogueTriggers[2].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 3)
        {
            dialogueTriggers[6].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 4)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 5)
        {
            dialogueTriggers[3].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 6)
        {
            startingUp = true;
            FlashArrow("Seesaw Arrow 11");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialLevel == 1)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<SeesawController>().GetTilt() > 0.5)
                {
                    TurnOffArrow("Seesaw Arrow 3");
                }
                else if (seesaw.GetComponent<SeesawController>().GetTilt() < -0.5)
                {
                    TurnOffArrow("Seesaw Arrow 2");
                }
                
                if (seesaw.GetComponent<SeesawController>().CheckDraggedStillBalanced())
                {
                    waitForFirstDrag = false;
                    DraggedCorrectly();
                }

            }
            else if (waitForSecondDrag)
            {
                if (seesaw.GetComponent<SeesawController>().GetTilt() > 0.5)
                {
                    TurnOffArrow("Seesaw Arrow 3");
                    checkArrow = true;
                }
                else if (seesaw.GetComponent<SeesawController>().GetTilt() < -0.5)
                {
                    TurnOffArrow("Seesaw Arrow 10");
                    checkArrow = true;
                }
                
                if (checkArrow)
                {
                    if (seesaw.GetComponent<SeesawController>().CheckDraggedStillBalanced2())
                    {
                        waitForSecondDrag = false;
                        TurnOffArrow("Seesaw Arrow 3");
                        TurnOffArrow("Seesaw Arrow 10");
                        DraggedCorrectly2();
                    }
                }
            }
        }
        else if (tutorialLevel == 2)
        {
            if (waitForFirstDrag)
            {
                // seesaw.transform.Find("RHSPositive").GetChild(0).gameObject.SetActive(true);
                if (flash)
                {
                    StartCoroutine(FlashSide(seesaw.transform.Find("RHSPositive")));
                    flash = false;
                }

                if (seesaw.GetComponent<SeesawController>().CheckDraggedToPositive())
                {
                    waitForFirstDrag = false;
                    DraggedToyOver();
                    seesaw.transform.Find("RHSPositive").GetChild(0).gameObject.SetActive(false);
                }
            }
            if (waitForSecondDrag)
            {
                // seesaw.transform.Find("LHSNegative").GetChild(0).gameObject.SetActive(true);
                if (flash)
                {
                    StartCoroutine(FlashSide(seesaw.transform.Find("LHSNegative")));
                    flash = false;
                }
                // StartCoroutine(FlashSide(seesaw.transform.Find("LHSNegative")));
                
                if (seesaw.GetComponent<SeesawController>().CheckDraggedToNegative())
                {
                    waitForSecondDrag = false;
                    DraggedToyOver2();
                    seesaw.transform.Find("LHSNegative").GetChild(0).gameObject.SetActive(false);
                    waitForThirdDrag = true;
                }
            }
            if (seesaw.transform.Find("LHSPositive").gameObject.GetComponent<SeesawSide>().NumValues() == 2 && waitForThirdDrag)
            {
                TurnOffArrow("Seesaw Arrow 8");
                FlashArrow("Seesaw Arrow 11");
                waitForThirdDrag = false;
            }
        }
        else if (tutorialLevel == 4)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.transform.Find("RHSPositive").GetChild(1).childCount == 2)
                {
                    waitForFirstDrag = false;
                    DoubleClicked();
                }
            }
            if (waitForSecondDrag)
            {
                if (seesaw.transform.Find("LHSPositive").GetChild(1).childCount == 1 && seesaw.transform.Find("RHSNegative").GetChild(1).childCount == 1)
                {
                    waitForSecondDrag = false;
                    DraggedOver();
                }
            }

            if (seesaw.transform.Find("RHSPositive").gameObject.GetComponent<SeesawSide>().NumValues() == 1 && ! seesaw.GetComponent<SeesawController>().GetDragging())
            {
                TurnOffArrow("Seesaw Arrow 9");
            }
            if (seesaw.transform.Find("RHSNegative").gameObject.GetComponent<SeesawSide>().NumValues() == 0 && ! seesaw.GetComponent<SeesawController>().GetDragging())
            {
                TurnOffArrow("Seesaw Arrow 13");
            }
        }
        else if (tutorialLevel == 6)
        {
            if (startingUp)
            {
                startingUp = false;
                GameObject bracket = checkLHSPositive.GetChild(1).GetChild(0).gameObject;
                Tut6Bracket check = bracket.AddComponent<Tut6Bracket>();
                dialogueTriggers[7].TriggerInitialDialogue();
                interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            if (finishedSecondDrag)
            {
                finishedSecondDrag = false;
                Expanded();
            }
        }
    }

    public void FinishedFirstDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (tutorialLevel == 1)
        {
            AnimateArrow("Seesaw Arrow 2");
            AnimateArrow("Seesaw Arrow 3");
            StartCoroutine(FlashLines("Flash 1"));
            StartCoroutine(FlashLines("Flash 2"));
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            waitForFirstDrag = true;
            flash = true;
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else if (tutorialLevel == 3)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            StartCoroutine(tutorialManager.EndDialogue());
        }
        else if (tutorialLevel == 4)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow 7");
            FlashArrow("Seesaw Arrow 8");
            waitForFirstDrag = true;
            dialogueNum = 2;
        }
        else if (tutorialLevel == 5)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow");
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow 4");
            FlashArrow("Seesaw Arrow 5");
            waitForFirstDrag = true;
        }
    }

    public void FinishedSecondDialogue()
    {
        if (tutorialLevel == 1)
        {
            AnimateArrow("Seesaw Arrow 3");
            AnimateArrow("Seesaw Arrow 10");
            StartCoroutine(FlashLines("Flash 3"));
            StartCoroutine(FlashLines("Flash 4"));
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            AnimateArrow("Seesaw Arrow 6");
            waitForSecondDrag = true;
            flash = true;
        }
        else if (tutorialLevel == 4)
        {
            AnimateArrow("Seesaw Arrow 12");
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 5)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow 2");
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow 4");
            FlashArrow("Seesaw Arrow 6");
            waitForSecondDrag = true;
        }
    }

    public void FinishedThirdDialogue()
    {
        if (tutorialLevel == 1)
        {
            FlashArrow("Seesaw Arrow 11");
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 2)
        {
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 4)
        {
            // interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 5)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            FlashArrow("Seesaw Arrow 3");
            waitForThirdDrag = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
    }

    public virtual void FinishedFourthDialogue()
    {
        if (tutorialLevel == 5)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
    }

    public void CurrentDialogue(int linesLeft)
    {
        if (tutorialLevel == 1)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 3)
                {
                    // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    // interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
                }
                else if (linesLeft == 2)
                {
                    // interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);

                    FlashArrow("Seesaw Arrow 7");
                    FlashArrow("Seesaw Arrow 9");
                }
                else if (linesLeft == 1)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    TurnOffArrow("Seesaw Arrow 7");
                    TurnOffArrow("Seesaw Arrow 9");
                }
            }
            else if (dialogueNum == 2)
            {
                if (linesLeft == 1)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    TurnOffArrow("Seesaw Arrow 2");
                }
            }
        }
        else if (tutorialLevel == 2)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 4)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    FlashArrow("Seesaw Arrow 4");
                }
                else if (linesLeft == 3)
                {
                    TurnOffArrow("Seesaw Arrow 4");
                    AnimateArrow("Seesaw Arrow 5");
                }
            }
            if (dialogueNum == 2)
            {
                if (linesLeft == 1)
                {
                    TurnOffArrow("Seesaw Arrow 5");
                    AnimateArrow("Seesaw Arrow 6");
                }
            }
            if (dialogueNum == 3)
            {
                if (linesLeft == 2)
                {
                    AnimateArrow("Seesaw Arrow 8");
                    StartCoroutine(FlashLines("Flash 5"));
                    StartCoroutine(FlashLines("Flash 6"));
                }
            }
        }
        else if (tutorialLevel == 4)
        {
            if (dialogueNum == 1)
            {
                /* if (linesLeft == 2)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 10").gameObject.SetActive(true);
                }
                else if (linesLeft == 1)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 10").gameObject.SetActive(false);
                } */
            }
            else if (dialogueNum == 2)
            {
                if (linesLeft == 2)
                {
                    AnimateArrow("Seesaw Arrow 9");
                    AnimateArrow("Seesaw Arrow 13");
                }
            }
        }
        else if (tutorialLevel == 6)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 3)
                {
                    TurnOffArrow("Seesaw Arrow 11");
                    FlashArrow("Seesaw Arrow 4");
                }
                if (linesLeft == 2)
                {
                    TurnOffArrow("Seesaw Arrow 4");
                }
            }
        }
    }

    // tutorial 1
    public void DraggedCorrectly()
    {
        dialogueNum = 2;
        dialogueTriggers[1].TriggerDialogue();
    }

    public void DraggedCorrectly2()
    {
        dialogueNum = 3;
        dialogueTriggers[5].TriggerDialogue();
    }

    // tutorial 2
    public void DraggedToyOver()
    {
        TurnOffArrow("Seesaw Arrow 5");
        dialogueNum = 2;
        dialogueTriggers[3].TriggerDialogue();
    }

    public void DraggedToyOver2()
    {
        TurnOffArrow("Seesaw Arrow 6");
        dialogueNum = 3;
        dialogueTriggers[4].TriggerDialogue();
    }

    // tutorial 6
    public void DoubleClicked()
    {
        dialogueTriggers[1].TriggerDialogue();
        TurnOffArrow("Seesaw Arrow 7");
        TurnOffArrow("Seesaw Arrow 8");
    }

    public void Combined()
    {
        dialogueTriggers[2].TriggerDialogue();
        TurnOffArrow("Seesaw Arrow 9");
        dialogueNum = 3;
    }

    public void DraggedOver()
    {
        dialogueTriggers[2].TriggerDialogue();
        TurnOffArrow("Seesaw Arrow 12");
        dialogueNum = 3;
    }

    // tutorial 11
    public void StartedBSO()
    {
        if (waitForFirstDrag)
        {
            dialogueTriggers[4].TriggerDialogue();
            TurnOffArrow("Seesaw Arrow");
            waitForFirstDrag = false;
        }
    }

    public void PressedOperation()
    {
        if (waitForSecondDrag)
        {
            TurnOffArrow("Seesaw Arrow 2");
            dialogueTriggers[5].TriggerDialogue();
            waitForSecondDrag = false;
        }
    }

    public void StartedNumber()
    {
        if (waitForThirdDrag)
        {
            TurnOffArrow("Seesaw Arrow 3");
            dialogueTriggers[6].TriggerDialogue();
            waitForThirdDrag = false;
        }
    }

    // tutorial 16
    public void FirstDrop()
    {
        TurnOffArrow("Seesaw Arrow 4");
        TurnOffArrow("Seesaw Arrow 5");
        waitForFirstDrag = false;
        dialogueTriggers[8].TriggerDialogue();
    }

    public void SuccessfullyExpanded()
    {
        finishedSecondDrag = true;
    }

    public void Expanded()
    {
        TurnOffArrow("Seesaw Arrow 4");
        TurnOffArrow("Seesaw Arrow 6");
        waitForSecondDrag = false;
        dialogueTriggers[9].TriggerDialogue();
    }

    public void SkipTutorial()
    {
        if (! doneTutorial)
        {
            interactivePanel.SetActive(false);
            tutorialManager.EndDialogueNow();
            skipButton.SetActive(false);
            doneTutorial = true;
        }
    }

    public void AnimateArrow(string arrowName)
    {
        interactivePanel.transform.Find(arrowName).gameObject.SetActive(true);
        interactivePanel.transform.Find(arrowName).gameObject.GetComponent<TutorialArrow>().SetAnimate(true);
    }

    public void TurnOffArrow(string arrowName)
    {
        interactivePanel.transform.Find(arrowName).gameObject.GetComponent<TutorialArrow>().SetAnimate(false);
        interactivePanel.transform.Find(arrowName).gameObject.GetComponent<TutorialArrow>().SetFlash(false);
        interactivePanel.transform.Find(arrowName).gameObject.SetActive(false);
    }

    public void FlashArrow(string arrowName)
    {
        interactivePanel.transform.Find(arrowName).gameObject.SetActive(true);
        interactivePanel.transform.Find(arrowName).gameObject.GetComponent<TutorialArrow>().SetFlash(true);
    }

    IEnumerator FlashLines(string name)
    {
        Transform obj = interactivePanel.transform.Find(name);
        bool check = true;
        for (int i = 0; i < 12; i++)
        {
            obj.gameObject.SetActive(check);
            check = !check;
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator FlashSide(Transform seesawside)
    {
        bool check = true;
        for (int i = 0; i < 10; i++)
        {
            seesawside.GetChild(0).gameObject.SetActive(check);
            check = !check;
            yield return new WaitForSeconds(0.4f);
        }
    }

}
