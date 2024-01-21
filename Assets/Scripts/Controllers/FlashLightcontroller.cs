using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    public Light flashLight;  // Reference to a Light component for the flashlight.
    public bool isFlashlightOn = false;  // A boolean flag to track if the flashlight is on or off.
    private Transform playerCamera;  // Reference to the player's camera.
    public float bigCrystalMaxIntensity = 30f; // Max intensity the BigCrystal can shine when light shinned on
    public float smallCrystalMaxIntensity = 20f; // Max Intensity the SmallCrystals can shine when lught shinned on


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
        if (UIManager.isGamePaused)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(1))  // Check if the right mouse button is clicked.
            {
                ToggleFlashlight();  // Toggle the flashlight on and off.
            }

            // Shine Crystals when Flashlight Shun 
            if (isFlashlightOn)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
                {
                    if (hit.collider.CompareTag("BigCrystal")) // Check if Big Crystal
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>(); // Find the light component 
                        if (pointLight != null) // Check if not null
                        {
                            IncreasePointLightIntensity(pointLight, bigCrystalMaxIntensity); // Crystal glow 
                        }
                    }
                    else if (hit.collider.CompareTag("SmallCrystal")) // Check if Small Crystal
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>(); // Find the light component
                        if (pointLight != null) // Check if not null
                        {
                            IncreasePointLightIntensity(pointLight, smallCrystalMaxIntensity); // Crystal glow 
                        }

                    }
                }
            }
        }
    }
     private void IncreasePointLightIntensity(Light pointLight, float maxIntensity)
    {
        pointLight.intensity += 10f * Time.deltaTime;  // Increase the intensity of the point light.
        pointLight.intensity = Mathf.Clamp(pointLight.intensity, 0f, maxIntensity);         // Clamp the intensity to the specified maximum value.
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
        Debug.Log("Flashlight is " + (isFlashlightOn ? "on" : "off"));  // Log the flashlight state.

    }
}