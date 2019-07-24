using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* The Negative part of an equation side.
 */
public class SeesawSide : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Equation sides can hold all types of draggable items.
    public Draggable.Slot typeOfItems = Draggable.Slot.All;
    public enum Slot {Positive, Negative};
    public Slot typeOfSide;
    public HintSystem hintSystem;

    private int capacity;
    private bool showColor;
    private bool firstTuts;
    private GameObject terms;
    private GameObject glow;

    void Start()
    {
        int level = FindObjectOfType<DataController>().GetDifficulty();
        if (level <= 5)
        {
            capacity = 8;
        }
        else
        {
            capacity = 5;
        }
        showColor = false;
        firstTuts = level <= 2;
        terms = this.transform.GetChild(1).gameObject;
        glow = this.transform.GetChild(0).gameObject;

        glow.SetActive(false);
    }

    // if Draggable object dropped onto this. Assuming all items dropped on it are Draggable.
    public void OnDrop(PointerEventData eventData)
    {
        glow.SetActive(false);
        showColor = false;

        int size = GetCurrentSize();

        if (size < capacity)
        {
            GameObject drop;
            if ( !(eventData.pointerDrag.name.EndsWith("(Clone)")))
            {
                // dragging from a restock zone then hopefully
                RestockZone restock = eventData.pointerDrag.GetComponent<RestockZone>();
                if (restock != null)
                {
                    drop = restock.newObject;
                }
                else
                {
                    drop = eventData.pointerDrag;
                }
            }
            else
            {
                drop = eventData.pointerDrag;
            }

            Draggable dragged = drop.GetComponent<Draggable>();
            if (dragged != null)
            {
                dragged.parentToReturnTo = terms.transform;

                if (typeOfSide == Slot.Positive)
                {
                    dragged.ShowOnPositiveSide();
                    Transform coefficient = drop.transform.Find("Coefficient");
                    if (coefficient != null)
                    {
                        Coefficient coef = coefficient.gameObject.GetComponent<Coefficient>();
                        if (coef.GetValue() < 0)
                        {
                            coef.NegativeCurrentValue();
                        }
                    }
                }
                else if (typeOfSide == Slot.Negative)
                {
                    dragged.ShowOnNegativeSide();
                    Transform coefficient = drop.transform.Find("Coefficient");
                    if (coefficient != null)
                    {
                        Coefficient coef = coefficient.gameObject.GetComponent<Coefficient>();
                        if (coef.GetValue() > 0)
                        {
                            coef.NegativeCurrentValue();
                        }
                    }
                }
            }
        }
        else
        {
            OverCapacity();
        }
    }

    // returns how much is currently on this side
    public int GetCurrentSize()
    {
        return NumValues() + NumVariables() + (int) (NumBrackets() * 2.5);
    }

    public bool CheckOverCapacity()
    {
        return GetCurrentSize() >= capacity;
    }

    public bool CheckOverCapacity(double add)
    {
        return GetCurrentSize() + add > capacity;
    }

    public void OverCapacity()
    {
        Debug.Log("Over Capacity");
        if (hintSystem != null)
        {
            hintSystem.SeesawSideOverflow();
        }
    }

    // get the number of variables currently on this side
    public int NumVariables()
    {
        int num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                num++;
            }
        }
        return num;
    }

    // get the number of values currently on this side
    public int NumValues()
    {
        int num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                num++;
            }
        }
        return num;
    }

    public int NumBrackets()
    {
        int num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                num++;
            }
        }
        return num;
    }

    // get the total value of all variables on this side
    public double NumericalVariables()
    {
        double num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                Transform coefficient = child.Find("Coefficient");
                double coef;
                if (coefficient != null)
                {
                    coef = coefficient.gameObject.GetComponent<Coefficient>().GetValue();
                } else {
                    coef = 1;
                }
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * coef;
            }
        }
        return num;
    }

    public double CoefficientVariables()
    {
        double num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                Transform coefficient = child.Find("Coefficient");
                double coef;
                if (coefficient != null)
                {
                    coef = coefficient.gameObject.GetComponent<Coefficient>().GetValue();
                } else {
                    coef = 1;
                }
                num = num + coef;
            }
        }
        return num;
    }

    // get the total value of all values on this side
    public double NumericalValues()
    {
        double num = 0;
        foreach(Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                Transform coefficient = child.Find("Coefficient");
                double coef;
                if (coefficient != null)
                {
                    coef = coefficient.gameObject.GetComponent<Coefficient>().GetValue();
                } else {
                    coef = 1;
                }
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * coef;
            }
        }
        return num;
    }

    public double NumericalBrackets()
    {
        double num = 0;
        foreach (Transform child in terms.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                num = num + child.gameObject.GetComponent<Bracket>().GetValue();
            }
        }
        return num;
    }

    public double TotalNumericalValue()
    {
        return NumericalValues() + NumericalVariables() + NumericalBrackets();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // if not tutorial and if dragging then glow panel
        bool currDragging = this.transform.parent.gameObject.GetComponent<SeesawController>().GetDragging();
        if (currDragging)
        {
            glow.SetActive(true);
            showColor = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // if not tutorial and if panel glowing then stop glow
        if (showColor)
        {
            glow.SetActive(false);
            showColor = false;
        }
    }
}
