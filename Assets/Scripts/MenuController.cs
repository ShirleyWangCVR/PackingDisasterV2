using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Controller for the Menu
 */
public class MenuController : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject levelselButton;

    private DataController dataController;
    private int currentMax;
    private bool atTut;

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentMax = dataController.GetQuestionType();
        atTut = dataController.GetAtTut();
    }

    // when start button is pressed
    public void FromBeginning()
    {
        dataController.StartLevel(1, true);
    }

    public void ResumeGame()
    {
        dataController.StartLevel(currentMax, atTut);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void ReviewTopics()
    {
        SceneManager.LoadScene("Review");
    }
}
