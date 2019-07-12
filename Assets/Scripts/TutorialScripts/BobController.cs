using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{
    private Animator anim;

    void Start() {
        anim = GetComponent(typeof(Animator)) as Animator;

        if (anim == null)
        {
            Debug.Log("did not find anim");
        }
    }

    public void dialogueEnterLeft() {
        Debug.Log("About to set trigger: dialogueEnterLeft.");

        anim = GetComponent(typeof(Animator)) as Animator;

        if (anim == null)
        {
            Debug.Log("did not find anim");
        }

        anim.SetTrigger("dialogueEnterLeft");
    }

    public void dialogueExitLeft() {
        Debug.Log("About to set trigger: dialogueExitLeft.");
        anim.SetTrigger("dialogueExitLeft");
    }
}
