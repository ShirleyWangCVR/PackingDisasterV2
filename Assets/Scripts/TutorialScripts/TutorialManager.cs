using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BobController bobCtrl;
    public DialogueModuleManager dialogueModMgr;

    public IEnumerator InitBobDialogue(string[] dialogueContents) {
        bobCtrl.dialogueEnterLeft();
        yield return new WaitForSeconds(2.2f);
        dialogueModMgr.InitDialogue("BOB", dialogueContents);
    }

    public IEnumerator BobDialogue(string[] dialogueContents) {
        dialogueModMgr.ContinueDialogue("BOB", dialogueContents);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(2f);
        dialogueModMgr.ExitDialogueBox();
        KickBob();
    }
    public void EndDialogueNow()
    {
        StartCoroutine(AbsoluteDialogueGone());
        KickBob();
    }

    public IEnumerator AbsoluteDialogueGone()
    {
        dialogueModMgr.ExitDialogueBox();
        yield return new WaitForSeconds(2f);
        dialogueModMgr.gameObject.SetActive(false);
    }

    public void KickBob() {
        bobCtrl.dialogueExitLeft();
    }
}
