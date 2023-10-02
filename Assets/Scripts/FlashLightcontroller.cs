using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    public Light flashLight;
    private bool isFlashlightOn = false;
    private Transform playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
        flashLight.enabled = false;

        // Find the camera
        playerCamera = transform.parent;

        // Make the flashlight a child of the camera.
        transform.SetParent(playerCamera);
    }

    // Update is called once per frame
    void Update()
    {
        //When Right click is placed
        if ( Input.GetMouseButtonDown(1) ) {
            ToggleFlashlight();
        }

    }

    //Flashlight Mechanics
    private void ToggleFlashlight ()
    {
        isFlashlightOn = !isFlashlightOn;
        flashLight.enabled = isFlashlightOn;
        Debug.Log("Flashlight is on!");
    }
}
