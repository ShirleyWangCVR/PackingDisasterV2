using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragCounter : MonoBehaviour
{
    public Text dragText;
    public GameObject cover;
    public Sprite emptyStar;
    public Image star1;
    public Image star2;

    private int numDrags;
    private int stars;
    private int boundaryOne;
    private int boundaryTwo;
    private float constantBy;
    
    // Start is called before the first frame update
    void Start()
    {
        stars = 2;
        numDrags = 0;
        dragText.text = numDrags.ToString();

        int level = FindObjectOfType<DataController>().GetQuestionType();
        if (level <= 3)
        {
            boundaryOne = 4;
        }
        else if (level <= 6)
        {
            boundaryOne = 6;
        }
        else
        {
            boundaryOne = 14; 
        }

        boundaryTwo = 2 * boundaryOne;

        constantBy = (float) 150 / (boundaryOne * 3); // TODO: finetune this so that it fits screen stretch

    }

    // Update is called once per frame
    void Update()
    {
        // dragText.text = numDrags.ToString();
    }

    public void DraggedOnce()
    {
        numDrags++;
        dragText.text = numDrags.ToString();

        if (numDrags <= boundaryOne * 3)
        {
            cover.GetComponent<RectTransform>().sizeDelta = new Vector2(numDrags * constantBy, 28);
        }

        if (numDrags == boundaryOne + 1)
        {
            star1.sprite = emptyStar;
            stars = 1;
        }

        if (numDrags == boundaryTwo + 1)
        {
            star2.sprite = emptyStar;
            stars = 0;
        }
    }

    public int GetStars()
    {
        return stars;
    }
}
