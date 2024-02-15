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

    RaycastHit hit; // Raycast hit var
    private RaycastHit sphereHit; // sphereCast Hit

    public float maxDistance = 1f; // MAx Distance when Flashlight intensity lowers
    public float minDistance = 2f; // Min Distance when flashlight intensity increases
    public float maxIntensity = 100f; //Max Intensity when close to an object 
    public float minIntensity = 70f; // Min Intensity when close to an object 

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
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit))
                {
                    if (hit.collider.CompareTag("BigCrystal")) // Check if Big Crystal
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>(); // Find the light component 
                        if (pointLight != null) // Check if not null
                        {
                            IncreaseCrystalLightIntensity(pointLight, bigCrystalMaxIntensity); // Crystal glow 
                        }
                    }
                    else if (hit.collider.CompareTag("SmallCrystal")) // Check if Small Crystal
                    {
                        Light pointLight = hit.collider.GetComponentInChildren<Light>(); // Find the light component
                        if (pointLight != null) // Check if not null
                        {
                            IncreaseCrystalLightIntensity(pointLight, smallCrystalMaxIntensity); // Crystal glow 
                        }

                    }
                    else if (hit.collider.CompareTag("InstructionMural")) // Check if Instruction Mural
                    {
                        Debug.Log("Hit an Instruction Mural");
                        // Check if the hit object has a renderer component
                        Renderer renderer = hit.collider.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            // Get the material of the renderer
                            Material material = renderer.material;

                            material.SetFloat("_Emissive_Strength", 1f);
                        }
                    }
                    
                    LightIntensityDistance(); // Gradual Increase/Decrease light intensity based on distance
                }
            }
        }
    }
    
    // Method to increase light intensity on crystals
     private void IncreaseCrystalLightIntensity(Light pointLight, float maxIntensity)
    {
        pointLight.intensity += 10f * Time.deltaTime;  // Increase the intensity of the light.
        pointLight.intensity = Mathf.Clamp(pointLight.intensity, 0f, maxIntensity); // Clamp the intensity to the specified maximum value.
    }
    
     // Method to toggle the flashlight on or off
    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
        Debug.Log("Flashlight is " + (isFlashlightOn ? "on" : "off"));  // Log the flashlight state.

    }
    
    // Method to gradually increase/decrease flashlight intensity based on distance 
    // To allow to see without blinding players
    private void LightIntensityDistance()
    {
        // Calculate the normalized distance based on the hit distance, min distance, and max distance
        // float normalizedDistance = Mathf.Clamp01((hit.distance - minDistance) / (maxDistance - minDistance * 0.5f));
        if (Physics.SphereCast(playerCamera.position, 50f, playerCamera.forward, out sphereHit, maxDistance))
        {
            // Use a smoother easing function to create subtler transitions in intensity
            //float easedIntensity = EaseInOut(normalizedDistance);
            float easedIntensity = Mathf.InverseLerp(minDistance, maxDistance, hit.distance);

            // Reduce the overall intensity range directly using a linear interpolation
            float smoothedIntensity = Mathf.Lerp(minIntensity, maxIntensity, easedIntensity);

            // Set the flashlight intensity to the smoothed value
            flashLight.intensity = smoothedIntensity;

            //Debug.Log("Hit distance: " + hit.distance);
            // Debug.Log("Hit GameObject name: " + hit.transform.name);
        }
    }

    // smoother ease-in-out function
    private float EaseInOut(float t)
    {
        // Cubing 't' to accentuate the easing effect
        // This gives a cubic curve for smoother transitions
        
        // Then Calculating a cubic Hermite interpolation
        // The inner part creates a smooth curve with ease-in-out effect
        
        // Combining the cubic curve with the Hermite interpolation
        // This produces an ease-in-out effect with smooth acceleration and deceleration
        
        // All that into this statement
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }
    
}