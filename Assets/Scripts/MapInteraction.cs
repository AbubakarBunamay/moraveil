using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public GameObject floodObject; // Reference to the flood object

    public void InteractWithMap()
    {
        Disappear();
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
