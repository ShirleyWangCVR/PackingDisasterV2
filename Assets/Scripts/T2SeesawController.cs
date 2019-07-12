using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the Game Seesaw
 */
public class T2SeesawController : SeesawController
{
    public Text leftEquationText;
    public Text rightEquationText;
    public Text signText;
    // other variables inherited from SeesawController

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        tilt = 0;
        currentlyDragging = false;
        roundActive = true;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        dataController = FindObjectOfType<DataController>();
        prevEquation = "";
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        if (! currentlyDragging)
        {
            UpdateTilt();
            UpdateCurrentEquation();
        }
        UpdatePositions();
    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    public override void UpdateTilt()
    {
        // update current tilt
        double lhs = 0;
        double rhs = 0;

        lhs = leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
        rhs = rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();

        prevTilt = tilt;
        tilt = lhs - rhs;
        if (tilt < 0.05 && tilt > -0.05)
        {
            // due to floating point arithmetic errors
            tilt = 0;
        }

        if (tilt != prevTilt)
        {
            CheckTilt();
        }
    }

    // get total numerical value of right hand side
    public override double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public override double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // check if a variable is correctly isolated
    public override bool CheckIfComplete()
    {
        // check if there is only 1 variable on the left hand side with coefficient 1
        // and only one value on the other
        if (leftHandSidePositive.transform.GetChild(1).childCount == 1 && leftHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.GetChild(1).childCount == 0)
        {
            if (leftHandSidePositive.transform.GetChild(1).GetChild(0).Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() == 1)
            {
                if (rightHandSidePositive.transform.GetChild(1).childCount == 1 && rightHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && rightHandSideNegative.transform.GetChild(1).childCount == 0)
                {
                    return true;
                }
                else if (rightHandSideNegative.transform.GetChild(1).childCount == 1 && rightHandSideNegative.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && rightHandSidePositive.transform.GetChild(1).childCount == 0)
                {
                    return true;
                }
            }
        }

        if (rightHandSidePositive.transform.GetChild(1).childCount == 1 && rightHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.GetChild(1).childCount == 0)
        {
            if (rightHandSidePositive.transform.GetChild(1).GetChild(0).Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() == 1)
            {
                if (leftHandSidePositive.transform.GetChild(1).childCount == 1 && leftHandSidePositive.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && leftHandSideNegative.transform.GetChild(1).childCount == 0)
                {
                    return true;
                }
                else if (leftHandSideNegative.transform.GetChild(1).childCount == 1 && leftHandSideNegative.transform.GetChild(1).GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && leftHandSidePositive.transform.GetChild(1).childCount == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void AddBothSides(int num)
    {
        if (leftHandSidePositive.GetComponent<SeesawSide>().CheckOverCapacity() || rightHandSidePositive.GetComponent<SeesawSide>().CheckOverCapacity())
        {
            leftHandSidePositive.GetComponent<SeesawSide>().OverCapacity();
        }
        else
        {

            GameObject newObject = toyPool.GetObject();
            newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(num);
            newObject.transform.SetParent(leftHandSidePositive.transform.GetChild(1));
            newObject.GetComponent<Draggable>().ShowOnPositiveSide();
            newObject.GetComponent<Draggable>().parentToReturnTo = leftHandSidePositive.transform.GetChild(1);

            GameObject new2Object = toyPool.GetObject();
            new2Object.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(num);
            new2Object.transform.SetParent(rightHandSidePositive.transform.GetChild(1));
            new2Object.GetComponent<Draggable>().ShowOnPositiveSide();
            new2Object.GetComponent<Draggable>().parentToReturnTo = rightHandSidePositive.transform.GetChild(1);
        }
    }

    public void SubtractBothSides(int num)
    {
        if (leftHandSideNegative.GetComponent<SeesawSide>().CheckOverCapacity() || rightHandSideNegative.GetComponent<SeesawSide>().CheckOverCapacity())
        {
            leftHandSideNegative.GetComponent<SeesawSide>().OverCapacity();
        }
        else
        {
            GameObject newObject = toyPool.GetObject();
            newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(0 - num);
            newObject.transform.SetParent(leftHandSideNegative.transform.GetChild(1));
            newObject.GetComponent<Draggable>().ShowOnNegativeSide();
            newObject.GetComponent<Draggable>().parentToReturnTo = leftHandSideNegative.transform.GetChild(1);

            GameObject new2Object = toyPool.GetObject();
            new2Object.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(0 - num);
            new2Object.transform.SetParent(rightHandSideNegative.transform.GetChild(1));
            new2Object.GetComponent<Draggable>().ShowOnNegativeSide();
            new2Object.GetComponent<Draggable>().parentToReturnTo = rightHandSideNegative.transform.GetChild(1);
        }
    }

    public void MultiplyBothSides(int num)
    {
        foreach(Transform child in leftHandSidePositive.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in leftHandSideNegative.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSidePositive.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSideNegative.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }
    }

    public void DivideBothSides(int num)
    {
        foreach(Transform child in leftHandSidePositive.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in leftHandSideNegative.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSidePositive.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSideNegative.transform.GetChild(1))
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }
    }

    public override void UpdateCurrentEquation()
    {
        string lside = "";
        string rside = "";

        foreach(Transform child in leftHandSidePositive.transform.GetChild(1))
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem != Draggable.Slot.Dummy)
            {
                if (lside.Length > 0)
                {
                    lside = lside + " + ";
                }
                lside = lside + StringTerm(child);
            }
        }

        foreach(Transform child in leftHandSideNegative.transform.GetChild(1))
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem != Draggable.Slot.Dummy)
            {
                if (lside.Length > 0)
                {
                    lside = lside + " - ";
                }
                else {
                    lside = lside + "-";
                }
                lside = lside + StringTerm(child);
            }
        }

        if (lside.Length == 0)
        {
            lside = lside + "0";
        }

        foreach(Transform child in rightHandSidePositive.transform.GetChild(1))
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem != Draggable.Slot.Dummy)
            {
                if (rside.Length > 0)
                {
                    rside = rside + " + ";
                }
                rside = rside + StringTerm(child);
            }
        }

        foreach(Transform child in rightHandSideNegative.transform.GetChild(1))
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem != Draggable.Slot.Dummy)
            {
                if (rside.Length > 0)
                {
                    rside = rside + " - ";
                }
                else {
                    rside = rside + "-";
                }
                rside = rside + StringTerm(child);
            }
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



        /* else if (tilt > 0)
        {
            signText.text = ">";
        }
        else if (tilt < 0)
        {
            signText.text = "<";
        } */

    }

    public string StringTerm(Transform term)
    {
        string termString = "";

        if (term.Find("Coefficient") != null)
        {
            Fraction value = term.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();

            if (value < 0)
            {
                value = -value;
            }

            if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                if (value != 1)
                {
                    termString = termString + value.ToString();
                }
                termString = termString + "X";
            }
            else if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                termString = termString + value.ToString();
            }
            else if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                if (value != 1)
                {
                    termString = termString + value.ToString();
                }
                termString = termString + "(";

                string bracket = "";
                foreach(Transform kid in term.Find("TermsInBracket"))
                {
                    Fraction val = kid.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();

                    if (bracket.Length > 0)
                    {
                        bracket = bracket + " + ";
                    }

                    if (kid.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
                    {
                        if (val != 1)
                        {
                            bracket = bracket + val.ToString();
                        }
                        bracket = bracket + "X";
                    }
                    else if (kid.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
                    {
                        bracket = bracket + val.ToString();
                    }
                }
                termString = termString + bracket + ")";
            }

            return termString;
        }
        else
        {
            return "";
        }


    }
}
