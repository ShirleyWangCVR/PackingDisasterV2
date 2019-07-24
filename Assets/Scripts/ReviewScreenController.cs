using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReviewScreenController : MonoBehaviour
{
    public GameObject imageToShow;
    public ReviewTopic[] topics;
    public Sprite locked;
    public Text title;

    private DataController dataController;
    private string problemArea;

    // Start is called before the first frame update
    void Start()
    {
        // hide topics not yet available
        dataController = FindObjectOfType<DataController>();
        if (dataController != null)
        {
            problemArea = dataController.GetProblemArea();
            Debug.Log(problemArea);
            if (problemArea != "")
            {
                if (problemArea == "operations on both sides")
                {
                    ShowImage(topics[2].reviewImage, topics[2].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
                }
                else if (problemArea == "how to solve an equation")
                {
                    ShowImage(topics[4].reviewImage, topics[4].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
                }
                else if (problemArea == "moving from one side to the other")
                {
                    ShowImage(topics[3].reviewImage, topics[3].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
                }
                else if (problemArea == "expanding brackets")
                {
                    ShowImage(topics[9].reviewImage, topics[9].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
                }
                else if (problemArea == "coefficients")
                {
                    ShowImage(topics[7].reviewImage, topics[7].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text);
                }

                dataController.SetProblemArea("");
            }

            // get current level and set sprite = locked, Text.SetActive(false), set locked
            int level = dataController.GetDifficulty();
            int bound;
            if (level < 6)
            {
                bound = 5;
            }
            else if (level < 11)
            {
                bound = 8;
            }
            else if (level < 16)
            {
                bound = 9;
            }
            else
            {
                bound = 10;
            }

            for (int i = 0; i < topics.Length; i++)
            {
                topics[i].SetUnlocked(true);
                /*
                if (i < bound)
                {
                    topics[i].SetUnlocked(true);
                }
                else
                {
                    topics[i].SetUnlocked(false);
                    topics[i].gameObject.transform.Find("Image").gameObject.GetComponent<Image>().sprite = locked;
                    topics[i].gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
                }
                */
            }
        }
    }

    public void ShowImage(Sprite image, string topicName)
    {
        imageToShow.SetActive(true);
        imageToShow.GetComponent<Image>().sprite = image;
        title.text = topicName;
    }

    public void CloseImage()
    {
        imageToShow.SetActive(false);
        title.text = "Review";
    }

    public void Back()
    {
        // start either level select or main menu depending on if they pressed
        // it from the menu or from end of a level
        string name = dataController.GetPreviousScene();
        if (name == "Menu")
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            SceneManager.LoadScene("Level Select");
        }
    }
}
