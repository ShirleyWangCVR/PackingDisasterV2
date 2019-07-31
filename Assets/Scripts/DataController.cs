using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/* Main Data Controller for game.
 * Contains and stores all data that needs to persist between scenes.
 */
public class DataController : MonoBehaviour
{
    // public DialogueData dialogue; // not in use
    public GameObject bearPrefab;
    public GameObject boxPrefab;
    public GameObject bearCoefPrefab;
    public GameObject boxCoefPrefab;
    public GameObject bracketPrefab;

    // list of all equations from the loaded json.
    private GameData allEquationData;
    private int[] levelIndexes;
    private int[] starsObtained;
    private int[] triedTutorial;
    private int[] tutorialStars;
    private int currentLevel; // current level
    private bool currentlyAtTutorial; // are we currently at a tutorial
    private int type; // highest type achieved
    private bool highestTutorial; // at the highest type achieved are we currently at a tutorial
    
    // private int levelsCompleted; // use this to set how many levels available on level select
    private string equationDataFileName = "equations.json";
    private PlayerOverallData playerLog;
    private string playerDataFileName;
    private PlayerMovesData currLevelData;
    private bool submittedCurrRound;
    private int prevStars;
    private string currentProblemArea;
    private string[] prevScenes;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        SceneManager.LoadScene("Menu");
        currentLevel = 1;
        type = 1;
        currentlyAtTutorial = true;
        highestTutorial = true;
        currentProblemArea = "";
        prevScenes = new string[2];

        playerDataFileName = "playerData" + Directory.GetFiles(Application.streamingAssetsPath, "*.json").Length.ToString() + ".json";

        levelIndexes = new int[7];
        starsObtained = new int[7];
        triedTutorial = new int[6];
        tutorialStars = new int[6];

        allEquationData.InitializeEquationsByString();

        SceneManager.sceneLoaded += OnSceneLoaded;
        playerLog = new PlayerOverallData();
        currLevelData = null;
        submittedCurrRound = false;

        // For some reason hardcoding the size at the start fixes resizing
        // issue between computers. It may look useless but removing the
        // sizeDelta setting will cause different resolution screens to render
        // the toys differently.
        // Setting localScale is so the toy scales with the canvas.
        Vector3 scale = FindObjectOfType<Canvas>().gameObject.transform.localScale;
        bearPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        bearPrefab.transform.localScale = scale;
        boxPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        boxPrefab.transform.localScale = scale;

        bearCoefPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 40);
        bearCoefPrefab.transform.localScale = scale;
        boxCoefPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 40);
        boxCoefPrefab.transform.localScale = scale;

        bracketPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 60);
        bracketPrefab.transform.localScale = scale;
    }

    // get current equation to show depending on provided level
    public EquationData GetCurrentEquationData(int level, bool tutorial)
    {
        if (tutorial)
        {
            if (level == 1)
            {
                return allEquationData.tut1Equation;
            }
            else if (level == 2)
            {
                return allEquationData.tut2Equation;
            }
            else if (level == 3)
            {
                return allEquationData.tut3Equation;
            }
            else if (level == 4)
            {
                return allEquationData.tut4Equation;
            }
            else if (level == 5)
            {
                return allEquationData.tut5Equation;
            }
            else if (level == 6)
            {
                return allEquationData.tut6Equation;
            }
        }
        else
        {
            if (level == 1)
            {
                return allEquationData.type1Equations[levelIndexes[0]];
            }
            else if (level == 2)
            {
                return allEquationData.type2Equations[levelIndexes[1]];
            }
            else if (level == 3)
            {
                return allEquationData.type3Equations[levelIndexes[2]];
            }
            else if (level == 4)
            {
                return allEquationData.type4Equations[levelIndexes[3]];
            }
            else if (level == 5)
            {
                return allEquationData.type5Equations[levelIndexes[4]];
            }
            else if (level == 6)
            {
                return allEquationData.type6Equations[levelIndexes[5]];
            }
            else if (level == 7)
            {
                return allEquationData.type7Equations[levelIndexes[6]];
            }
        }
        return allEquationData.tut1Equation;
    }

    public int GetTriedTutorial(int level)
    {
        return triedTutorial[level - 1];
    }

    public void SetTriedTutorial(int level, int num)
    {
        triedTutorial[level - 1] = num;
    }

    public void StartLevel(int level, bool tutorial)
    {
        // SetDifficulty(level, tutorial);
        currentLevel = level;
        currentlyAtTutorial = tutorial;

        if (tutorial)
        {
            if (level <= 3)
            {
                SceneManager.LoadScene("TutLevel1");
            }
            else if (level <= 7)
            {
                SceneManager.LoadScene("TutLevel2");
            }
            else
            {
                SceneManager.LoadScene("Ending");
            }
        }
        else
        {
            if (level <= 3)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                SceneManager.LoadScene("MainT2");
            }
        }

        /* if (level <= 2)
        {
            SceneManager.LoadScene("TutLevel1");
        }
        else if (level <= 5)
        {
            SceneManager.LoadScene("Main"); // levels 3 to 5
        }
        else if (level == 6 || level == 11 || level == 16)
        {
            SceneManager.LoadScene("TutLevel2"); // Tut Stage 4, coefficients
        }
        else if (level < 26)
        {
            SceneManager.LoadScene("MainT2");
        }
        else
        {
            SceneManager.LoadScene("Ending");
        } */
    }

    // only called when completed a question
    public void SubmitNewStars(int level, int stars, bool isTut)
    {
        // level = currentLevel;

        if (isTut)
        {
            if (stars > 0)
            {
                if (level == type)
                {
                    highestTutorial = false;
                }
            }
            
            int starsBefore = tutorialStars[level - 1];
            if (starsBefore <= stars)
            {
                tutorialStars[level - 1] = stars;
                stars = stars - starsBefore;
            }
            
            
            /* if (level <= 2)
            {
                int starsBefore = tutorialStars[level - 1];
                if (starsBefore <= stars)
                {
                    tutorialStars[level - 1] = stars;
                    stars = stars - starsBefore;
                }
            }
            else if (level == 6)
            {
                int starsBefore = tutorialStars[2];
                Debug.Log(starsBefore);
                if (starsBefore <= stars)
                {
                    tutorialStars[2] = stars;
                    stars = stars - starsBefore;
                }
            }
            else if (level == 11)
            {
                int starsBefore = tutorialStars[3];
                if (starsBefore <= stars)
                {
                    tutorialStars[3] = stars;
                    stars = stars - starsBefore;
                }
            }
            else if (level == 16)
            {
                int starsBefore = tutorialStars[4];
                if (starsBefore <= stars)
                {
                    tutorialStars[4] = stars;
                    stars = stars - starsBefore;
                }
            } */
        }
        else
        {
            int previousStars = prevStars;
            prevStars = stars;
            stars = stars - previousStars;
            if (stars < 0)
            {
                stars = 0;
            }
        }

        starsObtained[level - 1] = starsObtained[level - 1] + stars;

        int bound = 15;
        if (level <= 2)
        {
            bound = 10;
        }

        if (starsObtained[level - 1] >= bound)
        {
            starsObtained[level - 1] = bound;
            if (level == type)
            {
                type++;
                highestTutorial = true;
                // TODO: find a way to figure out when this is turned false
            }
        }
    }

    // go to the next equation's index after completing a level
    public void GoToNextIndex(bool isTut, int level)
    {
        if (! isTut)
        {
            levelIndexes[level - 1]++;
            if (levelIndexes[level - 1] == 7) // TODO: capped at 4 because only 4 equations mostly, eventually up to whatever's in the json
            {
                levelIndexes[level - 1] = 0;
            }
            prevStars = 0;
        }
    }

    public int GetTutorialStars(int index)
    {
        return tutorialStars[index];
    }

    public int GetStars(int level)
    {
        return starsObtained[level - 1];
    }

    public int GetDifficulty()
    {
        return currentLevel;
    }

    public bool GetCurrentTut()
    {
        return currentlyAtTutorial;
    }

    public int GetQuestionType()
    {
        return type;
    }

    public bool GetAtTut()
    {
        return highestTutorial;
    }

    public void SetTypeQuestion(int newType)
    {
        type = newType;
    }

    public string GetProblemArea()
    {
        string problem = currentProblemArea;
        currentProblemArea = ""; // since this is only used for the review screen
        return problem;
    }

    public void SetProblemArea(string problem)
    {
        currentProblemArea = problem;
    }

    // load game data from json
    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, equationDataFileName);

        if (File.Exists(filePath))
        {
            string jsonGameData = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonGameData);

            allEquationData = loadedData;
        } else {
            Debug.LogError("Cannot Load Game Data");
        }
    }

    public string GetPreviousScene()
    {
        return prevScenes[0];
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // update the player log every time a scene changes
        prevScenes[0] = prevScenes[1];
        prevScenes[1] = scene.name;

        if (scene.name == "Menu")
        {
            SubmitCurrentRoundData();
        }
        else if (scene.name.StartsWith("Tut") || scene.name.StartsWith("Main"))
        {
            Debug.Log("Logging " + currentLevel.ToString() + " rounddata");
            currLevelData = new PlayerMovesData(currentLevel);
            submittedCurrRound = false;
        }
    }

    // TODO: data logging method needs to be changed / updated
    // at the end of every round, submit current round data
    public void SubmitCurrentRoundData()
    {
        if (! submittedCurrRound)
        {
            if (currLevelData != null)
            {
                playerLog.NewRoundData(currLevelData);
                SaveCurrentPlayerData();
                submittedCurrRound = true;
            }
        }
    }

    // save the current player log to the json
    public void SaveCurrentPlayerData()
    {
        Debug.Log("Saving current player data");
        string dataAsJson = JsonUtility.ToJson(playerLog);
        string filePath = Path.Combine(Application.streamingAssetsPath, playerDataFileName);
        File.WriteAllText(filePath, dataAsJson);
    }

    public void SubmitEquation(string equation)
    {
        currLevelData.AddEquationLog(equation);
    }

    public void StoreEndRoundData(float time, bool done, int stars, string reason)
    {
        currLevelData.SubmitEndRound(time, done, stars, reason);
    }

    public void StoreDragData(string dragData)
    {
        currLevelData.AddDragLog(dragData);

        HintSystem hintSystem = FindObjectOfType<HintSystem>();
        if (hintSystem != null)
        {
            hintSystem.AddDragInfo(dragData);
        }
        FindObjectOfType<DragCounter>().DraggedOnce();
    }














    // methods past this point work but currently not in use

    /* public void SetNewStars(int level, int stars)
    {
        if (starsObtained[level - 1] < stars)
        {
            starsObtained[level - 1] = stars;
        }
    } */


    /*
    public int GetTotalStarsUpTo(int level)
    {
        int sum = 0;
        for (int i = 0; i < level; i++)
        {
            sum = sum + starsObtained[i];
        }
        return sum;
    } */

    /* public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

    public void SetLevelsCompleted(int newNum)
    {
        levelsCompleted = newNum;
    } */

    // load dialogue data from json
    /* private void LoadDialogueData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dialogueDataFileName);

        if (File.Exists(filePath))
        {
            string jsonDialogueData = File.ReadAllText(filePath);
            dialogue = JsonUtility.FromJson<DialogueData>(jsonDialogueData);

        } else {
            Debug.LogError("Cannot Load Dialogue Data");
        }
    }

    // submit a new score and store it if it's the highest
    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    // save current player progress in player prefs
    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

    // load current player progress
    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    } */


}
