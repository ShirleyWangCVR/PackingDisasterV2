using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip levelSelMusic;
    public AudioClip tutMusic;
    public AudioClip mainMusic;
    public AudioClip endMusic;
    
    private AudioSource audioSource;
    private string prevScene;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = this.gameObject.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        prevScene = "None";
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);

        if (scene.name != prevScene)
        {
            if (scene.name == "Menu")
            {
                audioSource.clip = menuMusic;
                audioSource.Play();
            }
            else if (scene.name == "Level Select")
            {
                audioSource.clip = levelSelMusic;
                audioSource.Play();
            }
            else if (scene.name == "Ending")
            {
                audioSource.clip = endMusic;
                audioSource.Play();
            }
            else if (scene.name.StartsWith("Tut"))
            {
                audioSource.clip = tutMusic;
                audioSource.Play();
            }
            else if (scene.name.StartsWith("Main"))
            {
                audioSource.clip = mainMusic;
                audioSource.Play();
            }
        }

        prevScene = scene.name;
    }

}
