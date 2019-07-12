using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BracketInsideCoefficient : MonoBehaviour, IDropHandler
{
    public bool droppedOn;
    
    private Bracket bracket;
    
    // Start is called before the first frame update
    void Start()
    {
        droppedOn = false;

        bracket = this.gameObject.transform.parent.parent.parent.GetComponent<Bracket>();
    }

    // when the bracket's external coefficient is dropped on it
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.name == "Coefficient")
        {
            if (! droppedOn)
            {
                droppedOn = true;
                bracket.TermDroppedOn();
                bracket.Invoke("CheckExpanded", 1f);
            }
        }
    }
}
