using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// optimize this at a later date once we figure out levels
public class LevelSelectController : MonoBehaviour
{
    public Button[] levelButtons;
    public GameObject[] topicLabels;
    public Button[] tutorialButtons;
    public GameObject[] starMeters;
    public Sprite[] stars;
    public Sprite redback;
    public GameObject endingButton;

    private DataController dataController;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        int levelsShow = dataController.GetTypeQuestion();

        Debug.Log(levelsShow);
        if (levelsShow == 6)
        {
            endingButton.SetActive(true);
        }

        for (int i = 0; i < 5; i++)
        {
            int starsCount = dataController.GetStars(i + 1);
            starMeters[i].transform.Find("Cover").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(292 * (15 - starsCount) / 15, 32);
            starMeters[i].transform.Find("Text").gameObject.GetComponent<Text>().text = starsCount.ToString() + "/15";

            if (i < levelsShow)
            {
                // unlock
                topicLabels[i].transform.Find("Text").gameObject.SetActive(true);
                topicLabels[i].transform.Find("Lock").gameObject.SetActive(false);
                topicLabels[i].GetComponent<Image>().sprite = redback;

                if (i == levelsShow - 1)
                {
                    if (i == 0)
                    {
                        if (starsCount < 3)
                        {
                            tutorialButtons[1].interactable = false;
                            tutorialButtons[1].transform.Find("Text").gameObject.SetActive(false);
                        }
                        if (starsCount < 6)
                        {
                            levelButtons[i].interactable = false;
                            levelButtons[i].transform.Find("Text").gameObject.SetActive(false);
                        }
                    }
                    else if (i < 4)
                    {
                        if (starsCount < 3)
                        {
                            levelButtons[i].interactable = false;
                            levelButtons[i].transform.Find("Text").gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                levelButtons[i].interactable = false;
                levelButtons[i].transform.Find("Text").gameObject.SetActive(false);

                if (i < 4)
                {
                    tutorialButtons[i + 1].interactable = false;
                    tutorialButtons[i + 1].transform.Find("Text").gameObject.SetActive(false);
                }
            }
        }
    }

    public void StartLevel(int level)
    {
        dataController.StartLevel(level);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ToEnding()
    {
        SceneManager.LoadScene("Ending");
    }
}
