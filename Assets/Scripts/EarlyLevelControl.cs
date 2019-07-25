using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for 1-3 to hide and deactive things not in use
 */
public class EarlyLevelControl : MonoBehaviour
{
    public GameObject[] drawers;
    public GameObject[] negativeSides;
    public GameObject equationPanel;

    private int level;
    private bool isTut;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<DataController>().GetDifficulty();
        isTut = FindObjectOfType<DataController>().GetCurrentTut();

        if (level < 3)
        {
            equationPanel.SetActive(false);
        }
        
        if (level == 1)
        {
            foreach (GameObject side in negativeSides)
            {
                side.SetActive(false);
            }
        }

        if (level == 2 && isTut)
        {
            foreach (GameObject draw in drawers)
            {
                draw.SetActive(false);
            }
        }
    }

    // TODO: call this when press skip tut
    public void TutorialSkipped()
    {
        foreach (GameObject draw in drawers)
        {
            draw.SetActive(true);
        }

        foreach (GameObject side in negativeSides)
        {
            side.SetActive(false);
        }
    }
}
