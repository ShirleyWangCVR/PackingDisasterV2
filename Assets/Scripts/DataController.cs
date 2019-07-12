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
    private EquationData[] allEquationsUsed;
    private int[] starsObtained;
    private int[] triedTutorial;
    private int currentLevel; // current level
    private int levelsCompleted; // use this to set how many levels available on level select
    private string equationDataFileName = "equations.json";
    private PlayerOverallData playerLog;
    private string playerDataFileName;
    private PlayerMovesData currLevelData;
    private bool submittedCurrRound;

    // Player Progress used to store between sessions. Currently not in use.
    // private PlayerProgress playerProgress;
    // private string dialogueDataFileName = "dialogueData.json";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        // LoadDialogueData();
        // LoadPlayerProgress();
        SceneManager.LoadScene("Menu");
        currentLevel = 1;
        levelsCompleted = 0;

        playerDataFileName = "playerData" + Directory.GetFiles(Application.streamingAssetsPath, "*.json").Length.ToString() + ".json";
        
        starsObtained = new int[25];
        for (int i = 0; i < allEquationsUsed.Length; i++)
        {
            allEquationsUsed[i].SetExpressionsByString();
            starsObtained[i] = 0;
        }

        triedTutorial = new int[5];
        for (int j = 0; j < 5; j++)
        {
            triedTutorial[j] = 0;
        }

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
    public EquationData GetCurrentEquationData(int level)
    {
        return allEquationsUsed[level - 1];
    }

    public int GetTriedTutorial(int level)
    {
        if (level <= 2)
        {
            return triedTutorial[level - 1];
        }
        else if (level == 6)
        {
            return triedTutorial[2];
        }
        else if (level == 11)
        {
            return triedTutorial[3];
        }
        else if (level == 16)
        {
            return triedTutorial[4];
        }
        return 0;
    }

    public void SetTriedTutorial(int level, int num)
    {
        if (level <= 2)
        {
            triedTutorial[level - 1] = num;
        }
        else if (level == 6)
        {
            triedTutorial[2] = num;
        }
        else if (level == 11)
        {
            triedTutorial[3] = num;
        }
        else if (level == 16)
        {
            triedTutorial[4] = num;
        }
    }

    public void StartLevel(int level)
    {
        SetDifficulty(level);

        if (level <= 2)
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
        }
    }

    public void SetNewStars(int level, int stars)
    {
        if (starsObtained[level - 1] < stars)
        {
            starsObtained[level - 1] = stars;
        }
    }

    public int GetStars(int level)
    {
        return starsObtained[level - 1];
    }

    public int GetDifficulty()
    {
        return currentLevel;
    }

    public void SetDifficulty(int difficulty)
    {
        currentLevel = difficulty;
    }

    public int GetTotalStarsUpTo(int level)
    {
        int sum = 0;
        for (int i = 0; i < level; i++)
        {
            sum = sum + starsObtained[i];
        }
        return sum;
    }

    // load game data from json
    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, equationDataFileName);

        if (File.Exists(filePath))
        {
            string jsonGameData = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonGameData);

            allEquationsUsed = loadedData.equationData;
        } else {
            Debug.LogError("Cannot Load Game Data");
        }
    }

    public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

    public void SetLevelsCompleted(int newNum)
    {
        levelsCompleted = newNum;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // update the player log every time a scene changes
        
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
    }

    












    // methods past this point work but currently not in use

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
