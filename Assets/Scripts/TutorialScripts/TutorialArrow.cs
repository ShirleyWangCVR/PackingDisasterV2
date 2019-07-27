using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the animation for the arrow cause i dunno how to actually animate
public class TutorialArrow : MonoBehaviour
{
    public GameObject cover;
    public GameObject image;

    private Vector2 size;
    private float constantBy;
    private int at;
    private int wait;
    private int flashCount;
    private bool check;
    private bool animate;
    private bool flash;
    
    // Start is called before the first frame update
    void Start()
    {
        size = this.GetComponent<RectTransform>().sizeDelta;
        constantBy = (float) size.x / 50;
        at = 0;
        wait = 20;
        flashCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animate && wait == 20)
        {
            at++;
            float check = constantBy * at;
            cover.GetComponent<RectTransform>().sizeDelta = new Vector2(check, size.y);

            if (at == 50)
            {
                at = 0;
                wait = 0;
            }
        }
        else if (animate)
        {
            wait++;
        }

        if (flash && flashCount <= 90)
        {
            if (flashCount % 15 == 0)
            {
                check = !check;
                image.SetActive(check);
            }
            flashCount++;
        }
        else if (flashCount == 91)
        {
            flash = false;
        }
    }

    public void SetAnimate(bool go)
    {
        animate = go;
    }

    public void SetFlash(bool go)
    {
        flash = go;
        flashCount = 0;
    }
}
