using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// optimize this at a later date once we figure out levels
public class LevelSelectController : MonoBehaviour
{
    public GameObject[] levelButtons;
    public GameObject[] topicLabels;
    public Sprite[] stars;
    public Sprite redback;

    private DataController dataController;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        int levelsShow = dataController.GetLevelsCompleted();

        // eventually set up buttons according to how much they've completed.
        for (int i = 0; i < 25; i++)
        {
            levelButtons[i].transform.Find("Number").gameObject.GetComponent<Text>().text = (i + 1).ToString();

            int levelStars = dataController.GetStars(i + 1);
            levelButtons[i].transform.Find("Stars").gameObject.GetComponent<Image>().sprite = stars[levelStars];

            // check if i > levels completed, if so set not interactable and hide num and stars
            // uncomment this when actual people play, keep this commented for easy testing
            /* if (i > levelsShow)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
                levelButtons[i].transform.Find("Number").gameObject.SetActive(false);
                levelButtons[i].transform.Find("Stars").gameObject.SetActive(false);
            }  */
        }

        // to disable the locks on the next topic or make sure the next topic is locked
        for (int i = 1; i < 5; i++)
        {
            if (dataController.GetTotalStarsUpTo(5 * i) >= 12 * i && levelsShow >= 5 * i)
            {
                // show next topic as not locked
                topicLabels[i].transform.Find("Text").gameObject.SetActive(true);
                topicLabels[i].transform.Find("Lock").gameObject.SetActive(false);
                topicLabels[i].GetComponent<Image>().sprite = redback;
            }
            // keep this commented for easy testing, uncommented for actual playing
            /* else
            {
                // make sure next level one is locked
                levelButtons[5 * i].GetComponent<Button>().interactable = false;
                levelButtons[5 * i].transform.Find("Number").gameObject.SetActive(false);
                levelButtons[5 * i].transform.Find("Stars").gameObject.SetActive(false);
            } */
        }
    }

    public void StartLevel(int level)
    {
        dataController.SetDifficulty(level);
        dataController.StartLevel(level);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
