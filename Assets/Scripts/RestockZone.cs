using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/* A zone where you click on to spawn another type of item, and drop items on
 * to get rid of them.
 */
public class RestockZone : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // the object pool of the game object that this zone will restock
    public SimpleObjectPool objectPool;
    public Draggable.Slot typeOfItems;
    public GameController gameController;
    public GameObject newObject;

    private Draggable childScript;
    private SoundEffectManager soundEffects;
    private DataController dataController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        soundEffects = FindObjectOfType<SoundEffectManager>();
        this.transform.GetChild(0).gameObject.SetActive(false);
        dataController = FindObjectOfType<DataController>();
    }

    public void OnBeginDrag(PointerEventData pointerDrag)
    {
        newObject = objectPool.GetObject(); 
        newObject.transform.position = this.transform.position + new Vector3(30, -30, 0);
        newObject.transform.SetParent(this.transform, true);

        // make sure orientation is correct
        int check = (int) Mathf.Round(newObject.transform.localScale.x);
        if (check == -1)
        {
            newObject.transform.Find("Image").localScale = new Vector3(1, 1, 1);
            newObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (typeOfItems == Draggable.Slot.Value)
        {
            newObject.GetComponent<HasValue>().SetValue(1);
        } 
        else if (typeOfItems == Draggable.Slot.Variable)
        {
            newObject.GetComponent<HasValue>().SetValue(gameController.GetEquation().variableValue);
        }

        if (newObject.transform.Find("Coefficient") != null)
        {
            Coefficient coef = newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();
            coef.SetValue(1);
        }

        childScript = newObject.GetComponent<Draggable>();
        childScript.ShowOnPositiveSide();
        childScript.Start();
        childScript.typeOfItem = typeOfItems;
        childScript.gameController = gameController;
        childScript.OnBeginDrag(pointerDrag);
        childScript.parentToReturnTo = this.gameObject.transform;
    }

    public void OnDrag(PointerEventData pointerDrag)
    {
        childScript.OnDrag(pointerDrag);

        // need to set this throughout entire drag so that it maintains to enddrag
        childScript.parentToReturnTo = this.gameObject.transform;
    }

    public void OnEndDrag(PointerEventData pointerDrag)
    {
        
        if (childScript.parentToReturnTo.name == this.transform.name)
        {
            childScript.OnEndDrag(pointerDrag);
            objectPool.ReturnObject(newObject);
            return;
        }
        
        childScript.OnEndDrag(pointerDrag);
        newObject = null;
        childScript = null;
    }

    // when an item is dropped on it get rid of it
    public void OnDrop(PointerEventData eventData)
    {        
        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (dragged != null)
        {            
            string dragData = eventData.pointerDrag.name + " was dragged from " + eventData.pointerDrag.GetComponent<Draggable>().parentName + " to " + this.transform.name;
            Debug.Log(dragData);
            dataController.StoreDragData(dragData);

            if (! eventData.pointerDrag.transform.name.EndsWith("Drawer"))
            {
                if (typeOfItems == dragged.typeOfItem)
                {
                    eventData.pointerDrag.GetComponent<Draggable>().SetIsDragging(false);
                    objectPool.ReturnObject(eventData.pointerDrag);
                    soundEffects.PlayRestocked();
                }
                else
                {
                    // play bzz
                    soundEffects.PlayBuzzer();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
