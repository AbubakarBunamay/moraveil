using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public List<GameObject> gameObjectToChange;

    public Material materialToChange;
    
    // Interact with the map function 
    public void InteractWithMap()
    {
        foreach (GameObject objToChange in gameObjectToChange)
        {
            ChangeMaterialOftheObject(objToChange);
        }
        Disappear();
    }
    private void ChangeMaterialOftheObject(GameObject objToChange)
    {
        if (objToChange != null)
        {
            Renderer render = objToChange.GetComponent<Renderer>();

            if (render != null)
            {
                render.material = materialToChange;
            }
            else
            {
                Debug.Log("No Renderer attached to " + objToChange.name);
            }
        }
        else
        {
            Debug.Log("No Object to be replaced");
        }
    }

    // Map Disappear 
    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
