using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    public Light flashLight;  // Reference to a Light component for the flashlight.
    public bool isFlashlightOn = false;  // A boolean flag to track if the flashlight is on or off.
    private Transform playerCamera;  // Reference to the player's camera.
    public GlowstickController glowstickController; // Reference to the GlowstickController script.


    void Start()
    {
        flashLight = GetComponent<Light>();  // Get the Light component of the object.
        flashLight.enabled = false;  // Initially, turn off the flashlight.

        playerCamera = transform.parent;  // Set the playerCamera reference to the parent of this object.
        transform.SetParent(playerCamera);  // Make this object a child of the playerCamera for positioning.

    }

    void Update()
    {
        // Check if the game is paused
        if (MoraveilSceneManager.isGamePaused)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(1))  // Check if the right mouse button is clicked.
            {
                ToggleFlashlight();  // Toggle the flashlight on and off.
                
            }

            // Check if the player is underwater and the glowstick controller is available.
            if (glowstickController != null && glowstickController.isInWater)
            {
                // Turn off the flashlight and turn on the glowstick.
                isFlashlightOn = false;
                flashLight.enabled = false;
                glowstickController.TurnOnGlowstick();
            }
        }
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
        Debug.Log("Flashlight is " + (isFlashlightOn ? "on" : "off"));  // Log the flashlight state.

    }
}