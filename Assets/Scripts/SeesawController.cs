using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the game seesaw.
 * Used for type 1 questions.
 */
public class SeesawController : MonoBehaviour
{
    public GameObject leftHandSidePositive;
    public GameObject rightHandSidePositive;
    public GameObject leftHandSideNegative;
    public GameObject rightHandSideNegative;
    public GameObject peg;
    public SimpleObjectPool toyPool;
    public SimpleObjectPool variablePool;
    public Text leftEquationText;
    public Text rightEquationText;
    public Text signText;
    public HintSystem hintSystem;
    
    // public Text equationText;

    protected bool currentlyDragging;
    protected double tilt;
    protected string originalSide; // original side of a thing being dragged
    protected bool roundActive;
    protected double prevTilt;
    protected Coroutine soundDanger;
    protected AudioSource audioSource;
    protected DataController dataController;
    protected string prevEquation;
    protected int level;

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        tilt = 0;
        currentlyDragging = false;
        roundActive = true;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        dataController = FindObjectOfType<DataController>();
        level = dataController.GetDifficulty();
        prevEquation = "";
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        if (! currentlyDragging && roundActive)
        {
            UpdateTilt();
            UpdateCurrentEquation();
        }
        UpdatePositions();
    }

    public void SetRoundActive(bool active)
    {
        roundActive = active;

        if (! active)
        {
            audioSource.Stop();
        }
    }

    public double GetTilt()
    {
        return tilt;
    }

    public void SetDragging(bool dragging, string side)
    {
        currentlyDragging = dragging;

        if (dragging)
        {
            originalSide = side;
        }

        // Allow things on other side to be allowed to be dropped onto
        bool right;
        bool left;
        if (originalSide == "right")
        {
            left = true;
            right = false;
        }
        else if (originalSide == "left")
        {
            left = false;
            right = true;
        }
        else
        {
            left = true;
            right = true;
        }

        if (left)
        {
            foreach(Transform child in leftHandSidePositive.transform.GetChild(1))
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }

            foreach(Transform child in leftHandSideNegative.transform.GetChild(1))
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }
        }

        if (right)
        {
            foreach(Transform child in rightHandSidePositive.transform.GetChild(1))
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }

            foreach(Transform child in rightHandSideNegative.transform.GetChild(1))
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }
        }

    }

    public bool GetDragging()
    {
        return currentlyDragging;
    }

    // make the seesaw tilt if it needs to
    protected virtual void UpdatePositions()
    {
        float rotateBy;
        if (currentlyDragging)
        {
            rotateBy = 0.01f;
        }
        else
        {
            rotateBy = 0.02f;
        }

        // tilt seesaw ominously
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }

        if (tilt > 0)
        {
            this.transform.Rotate(0, 0, rotateBy, Space.Self);
            peg.transform.Rotate(0, 0, rotateBy, Space.Self);
        }
        else if (tilt < 0)
        {
            this.transform.Rotate(0, 0, 0 - rotateBy, Space.Self);
            peg.transform.Rotate(0, 0, 0 - rotateBy, Space.Self);
        }
        else
        {   // tilt == 0
            // Unity doesn't move it by exact values so give it a slight bit of wiggle room when
            // returning to horizontal
            if (currangle > 0.1 || currangle < -0.1)
            {
                if (this.transform.rotation.eulerAngles.z < 180)
                {
                    this.transform.Rotate(0, 0, -0.1f, Space.Self);
                    peg.transform.Rotate(0, 0, -0.1f, Space.Self);
                } else
                {
                    this.transform.Rotate(0, 0, 0.1f, Space.Self);
                    peg.transform.Rotate(0, 0, 0.1f, Space.Self);
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    public virtual void UpdateTilt()
    {
        // update current tilt
        int lhs = 0;
        int rhs = 0;

        foreach(Transform child in leftHandSidePositive.transform.GetChild(1))
        {
            lhs = lhs + child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in leftHandSideNegative.transform.GetChild(1))
        {
            lhs = lhs - child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in rightHandSidePositive.transform.GetChild(1))
        {
            rhs = rhs + child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in rightHandSideNegative.transform.GetChild(1))
        {
            rhs = rhs - child.gameObject.GetComponent<HasValue>().GetValue();
        }

        prevTilt = tilt;
        tilt = lhs - rhs;

        if (tilt != prevTilt)
        {
            CheckTilt();
        }

    }

    // if it's tipped over more than 25 then the seesaw it too tipped over and they lose
    public bool FellOver()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = 360 - this.transform.rotation.eulerAngles.z;
        }

        return currangle > 20;
    }

    // check if a variable is correctly isolated
    public virtual bool CheckIfComplete()
    {
        // check if there is only 1 variable on the left hand side
        if (leftHandSidePositive.transform.GetChild(1).childCount == 1 && leftHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.GetChild(1).childCount == 0)
        {
            return rightHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 0 && rightHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
        }

        if (rightHandSidePositive.transform.GetChild(1).childCount == 1 && rightHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.GetChild(1).childCount == 0)
        {
            return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 0 && leftHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
        }

        return false;
    }

    // check if both sides of equation are equal
    public bool CorrectlyBalanced()
    {
        return tilt == 0;
    }

    // get total numerical value of right hand side
    public virtual double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() - rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public virtual double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() - leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    public virtual void UpdateCurrentEquation()
    {
        string lside = "";
        string rside = "";

        int lhsPosVars = leftHandSidePositive.GetComponent<SeesawSide>().NumVariables();
        int lhsPosValues = leftHandSidePositive.GetComponent<SeesawSide>().NumValues();
        if (lhsPosVars > 0)
        {
            if (lhsPosVars == 1)
            {
                lside = lside + "x";
            }
            else
            {
                lside = lside + lhsPosVars.ToString() + "x";
            }
        }
        if (lhsPosValues > 0)
        {
            if (lhsPosVars > 0)
            {
                lside = lside + " + ";
            }
            lside = lside + lhsPosValues.ToString();
        }

        int lhsNegVars = leftHandSideNegative.GetComponent<SeesawSide>().NumVariables();
        int lhsNegValues = leftHandSideNegative.GetComponent<SeesawSide>().NumValues();
        if (lhsNegVars > 0)
        {
            if (lside.Length > 0)
            {
                lside = lside + " - ";
            }
            else
            {
                lside = lside + "-";
            }

            if (lhsNegVars > 1)
            {
                lside = lside + lhsNegVars.ToString();
            }
            lside = lside + "x";
        }
        if (lhsNegValues > 0)
        {
            if (lside.Length > 0)
            {
                lside = lside + " - ";
            }
            else
            {
                lside = lside + "-";
            }
            lside = lside + lhsNegValues.ToString();
        }

        if (lside.Length == 0)
        {
            lside = lside + "0";
        }

        // right hand side calculation
        int rhsPosVars = rightHandSidePositive.GetComponent<SeesawSide>().NumVariables();
        int rhsPosValues = rightHandSidePositive.GetComponent<SeesawSide>().NumValues();
        if (rhsPosVars > 0)
        {
            if (rhsPosVars > 1)
            {
                rside = rside + rhsPosVars.ToString();
            }
            rside = rside + "x";
        }
        if (rhsPosValues > 0)
        {
            if (rhsPosVars > 0)
            {
                rside = rside + " + ";
            }
            rside = rside + rhsPosValues.ToString();
        }

        int rhsNegVars = rightHandSideNegative.GetComponent<SeesawSide>().NumVariables();
        int rhsNegValues = rightHandSideNegative.GetComponent<SeesawSide>().NumValues();
        if (rhsNegVars > 0)
        {
            if (rside.Length > 0)
            {
                rside = rside + " - ";
            }
            else
            {
                rside = rside + "-";
            }

            if (rhsNegVars > 1)
            {
                rside = rside + rhsNegVars.ToString();
            }
            rside = rside + "x";
        }
        if (rhsNegValues > 0)
        {
            if (rside.Length > 0)
            {
                rside = rside + " - ";
            }
            else
            {
                rside = rside + "-";
            }
            rside = rside + rhsNegValues.ToString();
        }

        if (rside.Length == 0)
        {
            rside = rside + "0";
        }

        string equation;
        if (tilt == 0)
        {
            signText.text = "=";
            equation = lside + " = " + rside;
        }
        else
        {
            signText.text = "≠"; // looks better with new display
            equation = lside + " ≠ " + rside;
        }

        if (prevEquation != equation)
        {
            dataController.SubmitEquation(equation);
        }
        prevEquation = equation;

        if (level > 2)
        {
            if (lside.Length > 30)
            {
                lside = "OVERFLOW";
                leftEquationText.color = Color.red;
            }
            else
            {
                leftEquationText.color = Color.white;
            }

            if (rside.Length > 30)
            {
                rside = "OVERFLOW";
                rightEquationText.color = Color.red;
            }
            else
            {
                rightEquationText.color = Color.white;
            }

            leftEquationText.text = lside;
            rightEquationText.text = rside;
        }
    }

    protected void CheckTilt()
    {
        if (tilt < 0.05 && tilt > -0.05)
        {
            StopCoroutine(soundDanger);
            audioSource.Stop();
        }
        else
        {
            soundDanger = StartCoroutine(PlayDanger());
        }
    }

    protected IEnumerator PlayDanger()
    {
        yield return new WaitForSeconds(2f);
        audioSource.Play();
        if (hintSystem != null)
        {
            hintSystem.SeesawTilting();
        }
    }

    public int LeftSideNumVariables()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() + leftHandSideNegative.GetComponent<SeesawSide>().NumVariables();
    }

    public int RightSideNumVariables()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumVariables() + rightHandSideNegative.GetComponent<SeesawSide>().NumVariables();
    }

    public int LeftSideNumValues()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumValues() + leftHandSideNegative.GetComponent<SeesawSide>().NumValues();
    }

    public int RightSideNumValues()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumValues() + rightHandSideNegative.GetComponent<SeesawSide>().NumValues();
    }

    public int LeftSideNumBrackets()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumBrackets() + leftHandSideNegative.GetComponent<SeesawSide>().NumBrackets();
    }

    public int RightSideNumBrackets()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumBrackets() + rightHandSideNegative.GetComponent<SeesawSide>().NumBrackets();
    }

    public int NumNegativeVariables()
    {
        return leftHandSideNegative.GetComponent<SeesawSide>().NumVariables() + rightHandSideNegative.GetComponent<SeesawSide>().NumVariables();
    }

    public double CoefficientVariables()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().CoefficientVariables() + rightHandSidePositive.GetComponent<SeesawSide>().CoefficientVariables();
    }

    // tutorial 1
    public bool CheckDraggedStillBalanced()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 1 && leftHandSidePositive.GetComponent<SeesawSide>().NumValues() == 1 && rightHandSidePositive.GetComponent<SeesawSide>().NumValues() == 4 && tilt == 0 && ! currentlyDragging;
    }

    public bool CheckDraggedStillBalanced2()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 1 && leftHandSidePositive.GetComponent<SeesawSide>().NumValues() == 0 && rightHandSidePositive.GetComponent<SeesawSide>().NumValues() == 3 && tilt == 0 && ! currentlyDragging;
    }

    // tutorial 2
    public bool CheckDraggedToPositive()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 1 && leftHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
    }

    public bool CheckDraggedToNegative()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumValues() == 0 && leftHandSideNegative.GetComponent<SeesawSide>().NumValues() == 1;
    }
}
