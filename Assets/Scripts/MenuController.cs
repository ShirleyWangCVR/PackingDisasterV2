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

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentMax = dataController.GetLevelsCompleted();
        if (currentMax > 0)
        {
            resumeButton.SetActive(true);
            levelselButton.SetActive(true);
        }
    }

    // when start button is pressed
    public void FromBeginning()
    {
        dataController.StartLevel(1);
    }

    public void ResumeGame()
    {
        dataController.StartLevel(currentMax + 1);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }
}
