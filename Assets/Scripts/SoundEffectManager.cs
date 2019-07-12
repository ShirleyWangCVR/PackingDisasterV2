using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip dingSfx;
    public AudioClip pickUpSfx;
    public AudioClip putDownToySfx;
    public AudioClip putDownBoxSfx;
    public AudioClip putDownBracketSfx;
    public AudioClip wrongSfx;
    public AudioClip expandedSfx;
    public AudioClip oneDraggedSfx;
    public AudioClip pickUpCoefSfx;
    public AudioClip clickedDiaSfx;
    public AudioClip clickedBSOSfx;
    public AudioClip completedBSOSfx;
    public AudioClip restockedSfx;
    
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = this.gameObject.GetComponent<AudioSource>();
    } 

    public void PlayDing()
    {
        audioSource.PlayOneShot(dingSfx, 0.5f);
    }

    public void PlayPickUpSfx()
    {
        audioSource.PlayOneShot(pickUpSfx, 6.0f);
    }

    public void PlayPutDownSfx(Draggable.Slot type)
    {
        if (type == Draggable.Slot.Value)
        {
            audioSource.PlayOneShot(putDownToySfx, 2.0f);
        }
        else if (type == Draggable.Slot.Variable)
        {
            audioSource.PlayOneShot(putDownBoxSfx, 2.0f);
        }
        else if (type == Draggable.Slot.Bracket)
        {
            audioSource.PlayOneShot(putDownBracketSfx, 2.0f);
        }   
    }

    public void PlayPickUpCoefSfx()
    {
        audioSource.PlayOneShot(pickUpCoefSfx, 7.0f);
    }

    public void PlayBuzzer()
    {
        audioSource.PlayOneShot(wrongSfx, 1.0f);
    }

    public void PlayExpanded()
    {
        audioSource.PlayOneShot(expandedSfx, 0.6f);
    }

    public void PlayOneDragged()
    {
        audioSource.PlayOneShot(oneDraggedSfx, 3.0f);
    }

    public void PlayClicked()
    {
        audioSource.PlayOneShot(clickedDiaSfx, 3.0f);
    }

    public void PlayClickedBSO()
    {
        audioSource.PlayOneShot(clickedBSOSfx, 5.0f);
    }

    public void PlayCompletedBSO()
    {
        audioSource.PlayOneShot(completedBSOSfx, 5.0f);
    }

    public void PlayRestocked()
    {
        audioSource.PlayOneShot(restockedSfx, 5.0f);
    }

}
