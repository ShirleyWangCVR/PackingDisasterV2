using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 * Mainly used for Type 1 questions.
 */
public class GameController : MonoBehaviour
{
    // public Text timeUsedText;
    public Text levelText;
    public GameObject seesaw;
    public SimpleObjectPool variablePool;
    public SimpleObjectPool toyPool;
    public FinishedPanelManager finishedDisplayManager;
    public TimeController timeController;

    protected DataController dataController;
    protected EquationData equation; // current equation being displayed
    protected bool currentlyDragging;
    protected bool notTutorial;
    protected bool roundActive;
    protected int level;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        level = dataController.GetDifficulty();
        equation = dataController.GetCurrentEquationData(level);
        levelText.text = "Level " + level.ToString();
        currentlyDragging = false;
        roundActive = true;

        // set up seesaw according to equation
        SetUpSeesaw();

        // if not tutorial then have a time limit
        /* if (! (level <= 2 || level == 6 || level == 11 || level == 16))
        {
            notTutorial = true;
            // timeUsedText.text = "Time Left: " + timeLeft.ToString();
        }
        else
        {
            notTutorial = false;
        } */
    }

    public EquationData GetEquation()
    {
        return equation;
    }

    // set up the seesaw according to the equation data
    protected virtual void SetUpSeesaw()
    {
        Expression lhs = equation.lhs;
        Expression rhs = equation.rhs;

        if (lhs.numVars > 0)
        {
            for (int i = 0; i < lhs.numVars; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive").GetChild(1);
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsPositive);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }
        else if (lhs.numVars < 0)
        {
            for (int i = 0; i < 0 - lhs.numVars; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative").GetChild(1);
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
                newVar.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }

        if (lhs.numValues > 0)
        {
            for (int i = 0; i < lhs.numValues; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive").GetChild(1);
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsPositive);
            }
        }
        else if (lhs.numValues < 0)
        {
            for (int i = 0; i < 0 - lhs.numValues; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative").GetChild(1);
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsNegative);
                newVar.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }

        if (rhs.numVars > 0)
        {
            for (int i = 0; i < rhs.numVars; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive").GetChild(1);
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsPositive);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }
        else if (rhs.numVars < 0)
        {
            for (int i = 0; i < 0 - rhs.numVars; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative").GetChild(1);
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
                newVar.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }

        if (rhs.numValues > 0)
        {
            for (int i = 0; i < rhs.numValues; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive").GetChild(1);
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsPositive);
            }
        }
        else if (rhs.numValues < 0)
        {
            for (int i = 0; i < 0 - rhs.numValues; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative").GetChild(1);
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsNegative);
                newVar.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // if not a tutorial then have time out and tip over
        if (roundActive) // && notTutorial
        {
            // if seesaw fell over end game
            if (seesaw.GetComponent<SeesawController>().FellOver())
            {
                EndRound("Scale Tipped");
            }
        }
    }

    // end the current round
    public void EndRound(string howEnded)
    {
        bool done = false;
        string reason = "";

        // deactivate game logic
        roundActive = false;
        seesaw.GetComponent<SeesawController>().SetRoundActive(false);

        int stars = 0;
        float time = timeController.GetCurrentTime();

        if (howEnded == "Finished Check")
        {
            if (seesaw.GetComponent<SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<SeesawController>().CorrectlyBalanced())
                {
                    // update total levels completed
                    int numCompleted = dataController.GetLevelsCompleted();
                    if (level > numCompleted)
                    {
                        dataController.SetLevelsCompleted(level);
                    }
                    stars = timeController.FinishedGameGetStars();
                    dataController.SetNewStars(level, stars);
                    done = true;

                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue, stars);
                }
                else
                {
                    // lost because wrong answer, get whatever they answered
                    int side = (int) seesaw.GetComponent<SeesawController>().GetLeftHandSideValue();
                    if (equation.variableValue != side)
                    {
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    } else {
                        side = (int) seesaw.GetComponent<SeesawController>().GetRightHandSideValue();
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    }
                    done = false;
                    reason = "Incorrect Value";
                }
            }
            else
            {
                finishedDisplayManager.DisplayNotYetBalanced();
                done = false;
                reason = "Not Isolated";
            }
        } else if (howEnded == "Scale Tipped")
        {
            finishedDisplayManager.DisplaySeesawTipped();
            done = false;
            reason = "Seesaw Tipped Over";
        }

        dataController.StoreEndRoundData(time, done, stars, reason);
        dataController.SubmitCurrentRoundData();
    }

    public void BackToMainMenu()
    {
        dataController.StoreEndRoundData(timeController.GetCurrentTime(), false, 0, "");
        SceneManager.LoadScene("Menu");
    }

    public void FinishedCheck()
    {
        EndRound("Finished Check");
    }

    public void SetDragging(bool dragging, string side)
    {
        currentlyDragging = dragging;
        seesaw.GetComponent<SeesawController>().SetDragging(dragging, side);
    }

    // pressed try question again button
    public void TryAgain()
    {
        // restart scene with the same equation
        dataController.StartLevel(level);
    }

    // move onto next question
    public void NextQuestion()
    {
        // tell DataController to move to next question and then load main scene again
        if (level > dataController.GetLevelsCompleted())
        {
            dataController.SetLevelsCompleted(level);
        }

        if (level % 5 == 0 && level < 25)
        {
            SceneManager.LoadScene("Level Select");
        }
        else
        {
            dataController.StartLevel(level + 1);
        }
    }
}
