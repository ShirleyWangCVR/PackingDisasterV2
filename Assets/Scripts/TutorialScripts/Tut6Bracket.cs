using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut6Bracket : MonoBehaviour
{
    public int numDroppedOn;
    public TutorialController tutController;
    
    // Start is called before the first frame update
    void Start()
    {
        numDroppedOn = 0;   
        tutController = FindObjectOfType<TutorialController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numDroppedOn != this.gameObject.GetComponent<Bracket>().numDroppedOn && this.gameObject.GetComponent<Bracket>().numDroppedOn == 1)
        {
            numDroppedOn = this.gameObject.GetComponent<Bracket>().numDroppedOn;
            tutController.FirstDrop();
        }
        else if (numDroppedOn != this.gameObject.GetComponent<Bracket>().numDroppedOn && this.gameObject.GetComponent<Bracket>().numDroppedOn == 2)
        {
            numDroppedOn = this.gameObject.GetComponent<Bracket>().numDroppedOn;
            tutController.SuccessfullyExpanded();
        }
    }
}
