using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* A simple object pool that keeps a stack of inactive objects to pop
 * off when we need more or less of the object to make the game performance slightly more efficient.
 * Since this way we don't have to continually instantiate and destroy extra objects
 */
public class SimpleObjectPool : MonoBehaviour
{
    // the prefab that this object pool returns instances of
    public GameObject prefab;
    public GameObject canvas;
    // collection of currently inactive instances of the prefab
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Returns an instance of the prefab
    public GameObject GetObject() 
    {
        GameObject spawnedGameObject;

        // if there is an inactive instance of the prefab ready to return, return that
        if (inactiveInstances.Count > 0) 
        {
            // remove the instance from the collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        // otherwise, create a new instance
        else 
        {
            spawnedGameObject = (GameObject) GameObject.Instantiate(prefab);

            // add the PooledObject component to the prefab so we know it came from this pool
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        // enable the instance
        spawnedGameObject.SetActive(true);
        spawnedGameObject.transform.SetParent(canvas.transform);
        spawnedGameObject.GetComponent<Draggable>().ShowOnPositiveSide();
                
        spawnedGameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        // return a reference to the instance
        return spawnedGameObject;
    }

    // Return an instance of the prefab to the pool
    public void ReturnObject(GameObject toReturn) 
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // if the instance came from this pool, return it to the pool
        if(pooledObject != null && pooledObject.pool == this)
        {
            
            Draggable d = toReturn.GetComponent<Draggable>();
            if (d != null)
            {
                d.DestroyPlaceholder();
            }
            
            // disable the instance
            toReturn.SetActive(false);
            toReturn.transform.SetParent(canvas.transform);

            // add the instance to the collection of inactive instances
            inactiveInstances.Push(toReturn);
        }
        // otherwise, just destroy it
        else
        {
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
            Destroy(toReturn);
        }
    }
}

// a component that simply identifies the pool that a GameObject came from
public class PooledObject : MonoBehaviour
{
    public SimpleObjectPool pool;
}