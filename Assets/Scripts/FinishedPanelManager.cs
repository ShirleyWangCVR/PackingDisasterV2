using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controller for the Finished Panel at the end of every level.
 */
public class FinishedPanelManager : MonoBehaviour
{
    public GameObject finishedDisplay;
    public Text userMessage;
    public GameObject boxDisplay;
    public GameObject toyDisplay;
    public SimpleObjectPool toyPool;
    public AudioClip youWinSfx;
    public AudioClip youLoseSfx;
    public Image[] starsDisplay;
    public Sprite fullStar;
    public GameObject reviewBubble;
    public Text reviewDialogue;
    public GameObject reviewButton;
    public GameObject nextQuestion;
    public GameObject starMeter;
    public Text stageText;

    private AudioSource audioSource;
    private DataController dataController;
    private int level;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        level = dataController.GetDifficulty();
    }

    // set finished display to if player wins
    public void DisplayCorrectlyBalanced(int correctValue, int timeStars, int dragStars)
    {
        audioSource.PlayOneShot(youWinSfx, 0.7f);
        nextQuestion.SetActive(true);
        stageText.text = "Stage " + level.ToString();
        SetStarMeter();

        int starsCount = dataController.GetStars(level);
        if (starsCount == 15)
        {
            nextQuestion.transform.Find("Text").gameObject.GetComponent<Text>().text = "Next Topic";
        }

        starsDisplay[0].sprite = fullStar;
        for (int i = 0; i < timeStars; i++)
        {
            starsDisplay[1 + i].sprite = fullStar;
        }
        for (int i = 0; i < dragStars; i++)
        {
            starsDisplay[3 + i].sprite = fullStar;
        }

        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Correctly " + correctValue.ToString() + " in the Box! You Win!";
        finishedDisplay.transform.Find("Question").gameObject.SetActive(false);

        if (dataController.GetDifficulty() < 6)
        {
            if (correctValue > 0)
            {
                for (int i = 0; i < correctValue; i++)
                {
                    GameObject toy = toyPool.GetObject();
                    toy.transform.SetParent(toyDisplay.transform);
                    toy.GetComponent<Draggable>().ShowOnPositiveSide();
                }
            }
            else if (correctValue < 0)
            {
                for (int i = 0; i < 0 - correctValue; i++)
                {
                    GameObject toy = toyPool.GetObject();
                    toy.transform.SetParent(toyDisplay.transform);

                    toy.GetComponent<Draggable>().ShowOnNegativeSide();
                }
            }
        }
        else
        {
            Destroy(toyDisplay.GetComponent<GridLayoutGroup>());

            GameObject toy = toyPool.GetObject();
            toy.transform.localScale = 2 * toy.transform.localScale;
            toy.transform.SetParent(toyDisplay.transform);
            toy.transform.position = toyDisplay.transform.position;
            toy.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(correctValue);
            if (correctValue > 0)
            {
                toy.GetComponent<Draggable>().ShowOnPositiveSide();
            }
            else
            {
                toy.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }
    }

    // set finished panel to if player lost by wrong answer
    public void DisplayWrongBalanced()
    {
        audioSource.PlayOneShot(youLoseSfx, 0.7f);
        finishedDisplay.SetActive(true);
        SetStarMeter();
        stageText.text = "Stage " + level.ToString();
        userMessage.text = "Try Again! Make Sure the Seesaw is Balanced.";
    }

    // set finished panel to if player didn't fully isolate answer
    public void DisplayNotYetBalanced()
    {
        audioSource.PlayOneShot(youLoseSfx, 0.7f);
        finishedDisplay.SetActive(true);
        SetStarMeter();
        stageText.text = "Stage " + level.ToString();
        userMessage.text = "Try Again! Remember To Simplify As Much As You Can.";
    }

    // set finished panel to if player lost by too unbalanced
    public void DisplaySeesawTipped()
    {
        audioSource.PlayOneShot(youLoseSfx, 0.7f);
        finishedDisplay.SetActive(true);
        SetStarMeter();
        stageText.text = "Stage " + level.ToString();
        userMessage.text = "The Seesaw Tipped Over! Try Again!";
    }

    public void SetProblemArea(string problemArea)
    {
        if (problemArea != "")
        {
            reviewBubble.SetActive(true);
            reviewDialogue.text = "Do you want to review " + problemArea + "?";
            reviewButton.SetActive(true);
        }
        dataController.SetProblemArea(problemArea);
    }

    public void GoToReview()
    {
        SceneManager.LoadScene("Review");
    }

    void SetStarMeter()
    {
        int starsCount = dataController.GetStars(level);
        starMeter.transform.Find("Cover").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(192 * (15 - starsCount) / 15, 32);
        starMeter.transform.Find("Text").gameObject.GetComponent<Text>().text = starsCount.ToString() + "/15";
    }
}
