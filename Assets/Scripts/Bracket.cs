using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bracket : MonoBehaviour
{
    public Expression expression;
    public int numTerms;
    public int numDroppedOn;
    public Sprite solidArrow;
    public Sprite dashedArrow;
    public Image arrow1;
    public Image arrow2;
    public GameObject arrow1Text;
    public GameObject arrow2Text;

    private bool expanded;
    private SoundEffectManager soundEffects;
    private DataController dataController;

    // Start is called before the first frame update
    void Start()
    {
        expanded = false;
        numDroppedOn = 0;
        numTerms = this.gameObject.transform.Find("TermsInBracket").childCount;
        soundEffects = FindObjectOfType<SoundEffectManager>();
        dataController = FindObjectOfType<DataController>();

        // give them the BracketInsideCoefficient component so that they can sense it being dropped on
        foreach (Transform child in this.gameObject.transform.Find("TermsInBracket"))
        {
            // child.gameObject.GetComponent<Draggable>().SetBracketStatus(true);
            child.gameObject.transform.Find("Coefficient").gameObject.AddComponent<BracketInsideCoefficient>();
            child.gameObject.GetComponent<Draggable>().inBracket = true;
        }

    }

    // if it's expanded then get rid of the bracket
    public void CheckExpanded()
    {
        if (expanded)
        {
            int i = 0;
            int numChildren = this.gameObject.transform.Find("TermsInBracket").childCount;
            while (i < numChildren)
            {
                Transform child = this.gameObject.transform.Find("TermsInBracket").GetChild(0);

                // TODO: put it where it should be
                // need to put negative values onto the negative sides and positive values onto the positive sides.

                Fraction newCoef = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();

                // assumes both terms inside the bracket are positive right now
                Transform parent = this.gameObject.transform.parent;
                child.SetParent(parent);

                // also need to set parentToReturnTo in Draggable
                child.gameObject.GetComponent<Draggable>().parentToReturnTo = parent;
                child.gameObject.GetComponent<Draggable>().inBracket = false;

                if (newCoef < 0)
                {
                    child.gameObject.GetComponent<Draggable>().ShowOnNegativeSide();
                }

                i++;
            }

            Destroy(this.gameObject);
        }
    }

    // when a term has been dropped on it react accordingly
    public void TermDroppedOn()
    {
        numDroppedOn++;
        // Debug.Log(numDroppedOn);
        string dragData = "Bracket Coefficient dragged onto Bracket Inside Term " + numDroppedOn.ToString();
        dataController.StoreDragData(dragData);

        if (numDroppedOn == numTerms)
        {
            // we have successfully expanded the bracket
            Debug.Log("Expanded");
            soundEffects.PlayExpanded();

            int i = 0;
            int numChildren = this.gameObject.transform.Find("TermsInBracket").childCount;
            while (i < numChildren)
            {
                Transform child = this.gameObject.transform.Find("TermsInBracket").GetChild(i);

                // multiply out coefficients
                Coefficient coef = child.Find("Coefficient").gameObject.GetComponent<Coefficient>();
                Fraction newCoef = coef.GetFractionValue() * this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
                coef.SetValue(newCoef);

                // reset as draggable
                // child.gameObject.GetComponent<Draggable>().SetBracketStatus(false);
                Destroy(child.gameObject.transform.Find("Coefficient").GetComponent<BracketInsideCoefficient>());

                i++;
            }

            // update arrows
            Coefficient coeff = this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();

            arrow1.gameObject.SetActive(true);
            arrow1.sprite = solidArrow;
            arrow1Text.SetActive(true);
            arrow1Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coeff.GetValue().ToString() + "x";

            arrow2.gameObject.SetActive(true);
            arrow2.gameObject.GetComponent<Image>().sprite = solidArrow;
            arrow2Text.SetActive(true);
            arrow2Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coeff.GetValue().ToString() + "x";

            coeff.SetValue(1);
            expanded = true;
        }
        else // first drop
        {
            soundEffects.PlayOneDragged();
            
            // show the arrows
            double coef = this.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();
            if (this.transform.Find("TermsInBracket").GetChild(0).Find("Coefficient").gameObject.GetComponent<BracketInsideCoefficient>().droppedOn)
            {
                // first item in bracket was dropped on
                arrow1.gameObject.SetActive(true);
                arrow1.sprite = solidArrow;
                arrow1Text.SetActive(true);
                arrow1Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coef.ToString() + "x";

                arrow2.gameObject.SetActive(true);
                arrow2.gameObject.GetComponent<Image>().sprite = dashedArrow;
            } else
            {
                arrow2.gameObject.SetActive(true);
                arrow2.sprite = solidArrow;
                arrow2Text.SetActive(true);
                arrow2Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coef.ToString() + "x";

                arrow1.gameObject.SetActive(true);
                arrow1.gameObject.GetComponent<Image>().sprite = dashedArrow;
            }
        }
    }

    // get total value of all values in bracket
    public double GetValue()
    {
        double coefficient = this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();

        double value = 0;
        foreach (Transform child in this.gameObject.transform.Find("TermsInBracket"))
        {
            value = value + child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() * child.gameObject.GetComponent<HasValue>().GetValue();
        }

        return coefficient * value;
    }
}
