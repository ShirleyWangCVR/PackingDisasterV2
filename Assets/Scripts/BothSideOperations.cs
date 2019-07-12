using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* A controller to process operations on both sides.
 */
public class BothSideOperations : MonoBehaviour
{
    public GameObject operationsPanel;
    public GameObject numbersPanel;
    public T2GameController gameController;
    public TutorialController tutController;
    // public Text LHS;
    // public Text RHS;
    public Text operationText;

    private string operation;
    private int number;
    private SoundEffectManager soundEffects;
    private bool processOperation;
    
    // Start is called before the first frame update
    void Start()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
        gameController = FindObjectOfType<T2GameController>();
        tutController = FindObjectOfType<TutorialController>();
        soundEffects = FindObjectOfType<SoundEffectManager>();
        // LHS.gameObject.SetActive(false);
        // RHS.gameObject.SetActive(false);
    }

    // Show choose operation panel
    public void InitiateBothSideOperations()
    {
        soundEffects.PlayClickedBSO();
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(false);
        processOperation = true;

        if (tutController != null)
        {
            tutController.StartedBSO();
        }
    }

    // Show choose number panel
    public void ChooseNumber()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(true);

        if (tutController != null)
        {
            tutController.PressedOperation();
        }
    }

    // Back to main play screen
    public void BackToMainScreen()
    {
        // audioSource.PlayOneShot(clickedSfx, 5.0f);
        soundEffects.PlayClickedBSO();
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
    }

    // Process the operation chosen
    public IEnumerator ProcessOperation()
    {
        processOperation = false;
        yield return new WaitForSeconds(1f);

        if (gameController != null)
        {
            gameController.ProcessBothSideOperation(operation, number);
        } 
        
        if (tutController != null)
        {
            tutController.StartedNumber();
        }
        // LHS.gameObject.SetActive(false);
        // RHS.gameObject.SetActive(false);
        soundEffects.PlayCompletedBSO();
        // audioSource.PlayOneShot(completedSfx, 5.0f);
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);    
    }

    // set the chosen operation
    public void SetOperation(string op)
    {
        // audioSource.PlayOneShot(clickedSfx, 5.0f);
        soundEffects.PlayClickedBSO();
        
        operation = op;
        // LHS.gameObject.SetActive(true);
        // RHS.gameObject.SetActive(true);
        ChooseNumber();

        if (op == "Addition")
        {
            // LHS.text = "+";
            // RHS.text = "+";
            operationText.text = "+";
        }
        else if (op == "Subtraction")
        {
            // LHS.text = "-";
            // RHS.text = "-";
            operationText.text = "-";
        }
        else if (op == "Multiplication")
        {
            // LHS.text = "x";
            // RHS.text = "x";
            operationText.text = "x";
        }
        else if (op == "Division")
        {
            // LHS.text = "÷";
            // RHS.text = "÷";
            operationText.text = "÷";
        }
    }

    // set the chosen number
    public void SetNumber(int num)
    {
        // audioSource.PlayOneShot(clickedSfx, 5.0f);
        if (processOperation)
        {
            processOperation = false;
            soundEffects.PlayClickedBSO();
        
            number = num;
            // LHS.text = LHS.text + num.ToString();
            // RHS.text = RHS.text + num.ToString();
            operationText.text = operationText.text + num.ToString();

            // wait one second then do it
            StartCoroutine(ProcessOperation());
        }
        
    }

}
