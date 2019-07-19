using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReviewTopic : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite reviewImage;

    private ReviewScreenController reviewController;
    private bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        reviewController = FindObjectOfType<ReviewScreenController>();
    }

    public void SetUnlocked(bool isUnlocked)
    {
        unlocked = isUnlocked;
    }

    public void ShowTheImage()
    {
        if (unlocked)
        {
            reviewController.ShowImage(reviewImage, this.transform.Find("Text").gameObject.GetComponent<Text>().text);
        }
    }
}
