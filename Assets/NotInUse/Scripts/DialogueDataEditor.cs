using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/* Create dialogue editor window for easy dialogue editing.
 */
public class DialogueDataEditor : EditorWindow
{
    public DialogueData dialogueData;

    private string dialogueDataProjectFilePath = "/StreamingAssets/dialoguedata.json";

    [MenuItem ("Window/Dialogue Data Editor")]
    static void Init()
    {
        DialogueDataEditor window = (DialogueDataEditor) EditorWindow.GetWindow(typeof(DialogueDataEditor));
        window.Show();
    }

    void OnGUI()
    {
        if (dialogueData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("dialogueData");
            
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data"))
            {
                SaveDialogueData();
            }
        }

        if (GUILayout.Button("Load Data"))
        {
            LoadDialogueData();
        }
    }
    
    private void LoadDialogueData()
    {
        string filePath = Application.dataPath + dialogueDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            dialogueData = JsonUtility.FromJson<DialogueData>(dataAsJson);

        } else {
            dialogueData = new DialogueData();
        }
    }

    private void SaveDialogueData()
    {
        string dataAsJson = JsonUtility.ToJson(dialogueData);
        string filePath = Application.dataPath + dialogueDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}
