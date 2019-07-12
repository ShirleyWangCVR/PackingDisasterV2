using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/* Create game editor window for easy dialogue editing.
 */
public class GameDataEditor : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;

    public GameData gameData;

    private string gameDataProjectFilePath = "/StreamingAssets/equations.json";
    
    [MenuItem ("Window/Game Data Editor")]
    static void Init()
    {
        GameDataEditor window = (GameDataEditor) EditorWindow.GetWindow(typeof(GameDataEditor));
        window.Show();
    }

    void OnGUI()
    {
        // GUILayout.BeginArea(new Rect(0, 0, 256, 600));
        GUILayout.BeginVertical();
 
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.ExpandHeight(true));
        
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
            
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load Data"))
        {
            LoadGameData();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        // GUILayout.EndArea();
    }
    
    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);

        } else {
            gameData = new GameData();
        }
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}
