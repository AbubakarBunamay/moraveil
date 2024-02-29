using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    [SerializeField]
    private Light flashLight;  // Reference to a Light component for the flashlight.
    
    // Crystal Glow Intensity 
    [SerializeField]
    private float bigCrystalMaxIntensity = 30f; // Max intensity the BigCrystal can shine when light shinned on
    [SerializeField]
    private float smallCrystalMaxIntensity = 20f; // Max Intensity the SmallCrystals can shine when lught shinned on
    
    // Flashlight Intensity & Distance
    [SerializeField]
    private float maxDistance = 1f; // MAx Distance when Flashlight intensity lowers
    [SerializeField]
    private float minDistance = 2f; // Min Distance when flashlight intensity increases
    [SerializeField]
    private float maxIntensity = 100f; //Max Intensity when close to an object 
    [SerializeField]
    private float minIntensity = 70f; // Min Intensity when close to an object 
    
    // Decal Fade Variables
    // Variable for fade speed
    [SerializeField]
    private float fadeSpeed = 5f;
    // Target fade factor
    private float targetFadeFactor = 0f;
    
    public bool isFlashlightOn = false;  // A boolean flag to track if the flashlight is on or off.
    private Transform playerCamera;  // Reference to the player's camera.x
    private RaycastHit hit; // Raycast hit var
    private RaycastHit sphereHit; // sphereCast Hit
    [SerializeField] private float minFalloff = 0.1f; // Adjust these values in the inspector
    [SerializeField] private float maxFalloff = 1f;   // Adjust these values in the inspector

    [SerializeField] private float minSpotAngle = 20f; // Minimum spot angle of the flashlight beam
    [SerializeField] private float maxSpotAngle = 60f; // Maximum spot angle of the flashlight beam
    [SerializeField] private float minRange = 5f; // Minimum range of the flashlight beam
    [SerializeField] private float maxRange = 20f; // Maximum range of the flashlight beam
    

    


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
                        // Get the DecalProjector component
                        var decalProjector = hit.collider.GetComponent<DecalProjector>();
                        if (decalProjector != null)
                        {
                            // Set the target fade factor to control opacity gradually
                            targetFadeFactor = 1f; 
                        }
                    }

                    // Gradually adjust the Decal fade factor
                    AdjustDecalFadeFactor();
                    
                    LightIntensityDistance(); // Gradual Increase/Decrease light intensity based on distance
                }
            }
        }
    }
    private void AdjustDecalFadeFactor()
    {
        // Get the DecalProjector component
        var decalProjector = hit.collider.GetComponent<DecalProjector>();
        if (decalProjector != null)
        {
            // Calculate the new fade factor based on the difference between current and target fade factors
            float newFadeFactor = Mathf.MoveTowards(decalProjector.fadeFactor, targetFadeFactor, fadeSpeed * Time.deltaTime);
            // Apply the new fade factor
            decalProjector.fadeFactor = newFadeFactor;
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
    [SerializeField] float spherecastRadius = 1f; // Radius of the spherecast
    [SerializeField] Color spherecastColor = Color.yellow; // Color of the spherecast

    // Method to gradually increase/decrease flashlight intensity based on distance 
    // To allow to see without blinding players
    private void LightIntensityDistance()
    {
        // Calculate the normalized distance based on the hit distance, min distance, and max distance
        // float normalizedDistance = Mathf.Clamp01((hit.distance - minDistance) / (maxDistance - minDistance * 0.5f));
        // if (Physics.SphereCast(playerCamera.position, 50f, playerCamera.forward, out sphereHit, maxDistance))
        // {
        //     // Use a smoother easing function to create subtler transitions in intensity
        //     //float easedIntensity = EaseInOut(normalizedDistance);
        //     float easedIntensity = Mathf.InverseLerp(minDistance, maxDistance, hit.distance);
        //
        //     // Reduce the overall intensity range directly using a linear interpolation
        //     float smoothedIntensity = Mathf.Lerp(minIntensity, maxIntensity, easedIntensity);
        //
        //     // Set the flashlight intensity to the smoothed value
        //     flashLight.intensity = smoothedIntensity;
        //
        //     //Debug.Log("Hit distance: " + hit.distance);
        //     // Debug.Log("Hit GameObject name: " + hit.transform.name);
        // }
        
        if (Physics.SphereCast(playerCamera.position, 0.5f, playerCamera.forward, out sphereHit, maxDistance))
        {
            float distance = sphereHit.distance;

            // Calculate the angle attenuation based on the distance
            float angleAttenuation = Mathf.Clamp01(distance / maxDistance);

            // Calculate the falloff factor for the flashlight intensity
            float falloffFactor = Mathf.Lerp(minFalloff, maxFalloff, angleAttenuation);

            // Calculate the intensity based on the falloff factor
            //float intensity = maxIntensity * falloffFactor;

            // Set the flashlight intensity to the calculated value
            //flashLight.intensity = intensity;

            // Calculate the adjusted spot angle and range
            float newSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, Mathf.Pow(angleAttenuation, 0.5f)); // Adjust spot angle
            //float newRange = Mathf.Lerp(minRange, maxRange, angleAttenuation); // Adjust range

            // Update the light component's properties
            flashLight.spotAngle = newSpotAngle;
            //flashLight.range = newRange;
        }
        else
        {
            // If no wall is hit, set intensity to minIntensity and reset spot angle and range
            //flashLight.intensity = minIntensity;
            //flashLight.spotAngle = maxSpotAngle;
            //flashLight.range = maxRange;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = spherecastColor;

        // Draw the spherecast
        Gizmos.DrawSphere(playerCamera.position, spherecastRadius);
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * maxDistance);
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