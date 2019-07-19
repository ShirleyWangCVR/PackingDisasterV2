using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarlyTutorialControl : MonoBehaviour
{
    public GameObject[] drawers;

    private int level;

    // Start is called before the first frame update
    void Start()
    {
        level = FindObjectOfType<DataController>().GetDifficulty();

        if (level == 2)
        {
            foreach (GameObject draw in drawers)
            {
                draw.SetActive(false);
            }
        }
    }

    // if we have skipped tutorials reactivate the drawers
}
