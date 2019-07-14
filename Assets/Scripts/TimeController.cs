using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Image[] showStars;
    public Image gearBack;
    public Image gearFront;
    public GameObject rack;
    // public Sprite[] stars;
    public Sprite emptyStar;

    private bool tutorial;
    private int currentStars;
    private float currentTime;
    private float currentLength;
    private float initialX;
    private float start;
    private float end;
    private float constantBy;
    private DataController dataController;
    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        gameController = FindObjectOfType<GameController>();
        initialX = rack.transform.position.x;
        start = this.transform.Find("Start").position.x;
        end = this.transform.Find("End").position.x;
        constantBy = (start - end) / 90;

        currentTime = 0;
        currentStars = 2;

        tutorial = dataController.GetDifficulty() <= 2 || dataController.GetDifficulty() == 6 || dataController.GetDifficulty() == 11 || dataController.GetDifficulty() == 16;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        
        if (! tutorial)
        {
            currentLength = currentTime * constantBy;

            if (currentTime > 30 && currentStars == 2)
            {
                showStars[0].sprite = emptyStar;
                currentStars = 1;
                // showStars.sprite = stars[2];
            }
            if (currentTime > 60 && currentStars == 1)
            {
                showStars[1].sprite = emptyStar;
                currentStars = 0;
            }

            if (currentTime <= 90)
            {
                rack.transform.position = new Vector3(initialX - currentLength, rack.transform.position.y, rack.transform.position.z);
                
                // more jittery movement
                // gearBack.transform.eulerAngles = new Vector3(0, 0, -2f * Mathf.Round(currentTime));
                // gearFront.transform.eulerAngles = new Vector3(0, 0, -2f * Mathf.Round(currentTime));

                // this looks okay on my big computer but i dunno how it'll be on other computers
                gearBack.transform.Rotate(0, 0, -0.16f, Space.Self);
                gearFront.transform.Rotate(0, 0, -0.16f, Space.Self);
            }
        }
    }

    public int FinishedGameGetStars()
    {
        tutorial = false;
        return currentStars;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
