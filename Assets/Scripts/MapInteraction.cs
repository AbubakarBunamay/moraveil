using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    // Interact with the map function 
    public void InteractWithMap()
    {
        Disappear();
    }

    // Map Disappear 
    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
