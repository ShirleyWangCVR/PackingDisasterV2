using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public LevelSelectController levelSelectController;
    public int level;
    
    // Start is called before the first frame update
    void Start()
    {
        levelSelectController = FindObjectOfType<LevelSelectController>();
    }

    public void StartLevel()
    {
        // string num = this.transform.Find("Number").gameObject.GetComponent<Text>().text;

        // int level = Int32.Parse(num);
        levelSelectController.StartLevel(level);
    }
}
