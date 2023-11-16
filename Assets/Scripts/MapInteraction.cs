using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    public GameObject floodObject; // Reference to the flood object

    public void InteractWithMap()
    {
        Disappear();

        // Get the FloodObjectMovement script from floodObject
        FloodWater floodWater = floodObject.GetComponent<FloodWater>();

        if (floodWater != null)
        {
            // Start flooding in the FloodObjectMovement script
            floodWater.StartFlooding();
        }
        else
        {
            Debug.LogError("FloodObjectMovement script not found on the floodObject.");
        }
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
