using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GetOperation : MonoBehaviour
{
    public enum Slot {Operation, Number};
    public Slot type;
    public BothSideOperations operationController;

    public void Start()
    {
        operationController = FindObjectOfType<BothSideOperations>();
    }

    public void ProcessPress()
    {
        if (type == Slot.Operation)
        {
            Debug.Log("Setting Operation");
            operationController.SetOperation(this.gameObject.name);
        } else if (type == Slot.Number)
        {
            operationController.SetNumber(int.Parse(this.gameObject.name));
        }
    }
}
