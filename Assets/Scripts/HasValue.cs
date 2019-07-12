using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GameObject that has a value to be balanced on the scale
 */
public class HasValue : MonoBehaviour
{
    public int value;
    // default to Value for now
    public Draggable.Slot typeOfItem = Draggable.Slot.Value;

    // Start is called before the first frame update
    void Start()
    {
        // default Value to 1, default Dummy to 0, Variables need to be set by Game Controller
        if (typeOfItem == Draggable.Slot.Value)
        {
            value = 1;
        } else if (typeOfItem == Draggable.Slot.Dummy)
        {
            value = 0;
        }
    }

    public void SetValue(int num)
    {
        value = num;
    }

    public int GetValue()
    {
        return value;
    }
}
