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
    public float bigCrystalMaxIntensity = 30f;
    public float smallCrystalMaxIntensity = 20f;


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

            // Shine Crystals when Flashlight Shun 
            if (isFlashlightOn)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
                {
                    if (hit.collider.CompareTag("BigCrystal"))
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>();
                        if (pointLight != null)
                        {
                            IncreasePointLightIntensity(pointLight, bigCrystalMaxIntensity);
                        }
                    }
                    else if (hit.collider.CompareTag("SmallCrystal"))
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>();
                        if (pointLight != null)
                        {
                            IncreasePointLightIntensity(pointLight, smallCrystalMaxIntensity);
                        }

                    }
                }
            }
        }
    }
     private void IncreasePointLightIntensity(Light pointLight, float maxIntensity)
    {
        pointLight.intensity += 10f * Time.deltaTime;  // Increase the intensity of the point light.

        // Clamp the intensity to the specified maximum value.
        pointLight.intensity = Mathf.Clamp(pointLight.intensity, 0f, maxIntensity);
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
        Debug.Log("Flashlight is " + (isFlashlightOn ? "on" : "off"));  // Log the flashlight state.

    }
}