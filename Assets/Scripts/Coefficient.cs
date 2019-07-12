using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* The coefficient of a term.
 */
public class Coefficient : MonoBehaviour
{
    public Text numeratorText;
    public Text denominatorText;
    public Text fractionLineText;
    public Text numberText;

    private Fraction value;

    // Start is called before the first frame update
    void Start()
    {
        // value.Denominator = 1;
    }

    // get the value of the coefficient as a double
    public double GetValue()
    {
        if (value.Denominator == 1)
        {
            return value.Numerator;
        }
        else
        {
            return value.ToDouble();
        }
    }

    // get the value of the coefficient as a fraction
    public Fraction GetFractionValue()
    {
        return value;
    }

    // set the value to the negative of its current value
    public void NegativeCurrentValue()
    {
        SetValue(-value);
    }

    // set the value according to the given fraction
    public void SetValue(Fraction newvalue)
    {
        value = newvalue;
        if (value.Denominator == 1)
        {
            numberText.gameObject.SetActive(true);
            numberText.text = ((int) GetValue()).ToString();

            numeratorText.gameObject.SetActive(false);
            denominatorText.gameObject.SetActive(false);
            fractionLineText.gameObject.SetActive(false);

        } else {
            numberText.gameObject.SetActive(false);

            numeratorText.gameObject.SetActive(true);
            numeratorText.text = value.Numerator.ToString();

            fractionLineText.gameObject.SetActive(true);

            denominatorText.gameObject.SetActive(true);
            denominatorText.text = value.Denominator.ToString();
        }
    }


    // set the value according to the given integer
    public void SetValue(int newvalue)
    {
        value = new Fraction(newvalue);

        numberText.gameObject.SetActive(true);
        numberText.text = ((int) GetValue()).ToString();

        numeratorText.gameObject.SetActive(false);
        denominatorText.gameObject.SetActive(false);
        fractionLineText.gameObject.SetActive(false);
    }
}
