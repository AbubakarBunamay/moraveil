using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    [SerializeField] private Light flashLight;  // Reference to a Light component for the flashlight.
    
    // Crystal Glow Intensity 
    [Header("Crystal Intensity")]
    [SerializeField] private float bigCrystalMaxIntensity = 30f; // Max intensity the BigCrystal can shine when light shinned on
    [SerializeField] private float smallCrystalMaxIntensity = 20f; // Max Intensity the SmallCrystals can shine when lught shinned on
    
    // Flashlight Modification
    [Header("Flashlight Mod")]
    [SerializeField] private float minFalloff = 0.1f; // Adjust these values in the inspector
    [SerializeField] private float maxFalloff = 1f;   // Adjust these values in the inspector
    [SerializeField] private float minSpotAngle = 20f; // Minimum spot angle of the flashlight beam
    [SerializeField] private float maxSpotAngle = 60f; // Maximum spot angle of the flashlight beam
    [SerializeField] private float minRange = 5f; // Minimum range of the flashlight beam
    [SerializeField] private float maxRange = 20f; // Maximum range of the flashlight beam
    [SerializeField] private float maxDistance = 1f; // MAx Distance when Flashlight intensity lowers
    [SerializeField] private float minDistance = 2f; // Min Distance when flashlight intensity increases
    [SerializeField] private float maxIntensity = 100f; //Max Intensity when close to an object 
    [SerializeField] private float minIntensity = 70f; // Min Intensity when close to an object 
    [SerializeField] float spherecastRadius = 2f; // Radius of the spherecast
    [SerializeField] Color spherecastColor = Color.red; // Color of the spherecast
    private RaycastHit hit; // Raycast hit var
    private RaycastHit sphereHit; // sphereCast Hit
    
    // Decal Fade Variables
    [Header("Decals Fade")]
    [SerializeField] private float fadeSpeed = 5f; // Variable for fade speed
    private float targetFadeFactor = 0f;// Target fade factor
    
    public bool isFlashlightOn = false;  // A boolean flag to track if the flashlight is on or off.
    private Transform playerCamera;  // Reference to the player's camera.x

    

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

            // Flashlight Interaction
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
                    else if (hit.collider.CompareTag("SymbolMural")) // Check if Instruction Mural
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
    public void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
    }

    // Method to gradually increase/decrease flashlight intensity based on distance 
    // To allow to see without blinding players
    private void LightIntensityDistance()
    {
        // Calculate the origin of the spherecast slightly higher and forward from the player's camera position
        Vector3 spherecastOrigin = playerCamera.position + playerCamera.forward * 0.01f + playerCamera.up * 1f;
        
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        int noCollideLayerMask = ~playerLayerMask;
        
        if (Physics.SphereCast(spherecastOrigin, spherecastRadius, playerCamera.forward, out sphereHit, maxDistance, noCollideLayerMask))
        {
            // Calculate the falloff factor based on whether hitting an object or not
            float falloffFactor = Mathf.Lerp(minFalloff, maxFalloff, Mathf.Clamp01(sphereHit.distance / maxDistance));

            // Calculate the intensity based on the falloff factor
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, EaseInOut(falloffFactor));

            // Set the flashlight intensity to the calculated value 
            flashLight.intensity = intensity;

            // Calculate the adjusted spot angle and range
            float angleAttenuation = Mathf.Clamp01(sphereHit.distance / maxDistance);
            float newSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, EaseInOut(falloffFactor));
            float newRange = Mathf.Lerp(minRange, maxRange, angleAttenuation);

            // Gradually adjust the flashlight's intensity, spot angle, and range based on distance
            flashLight.intensity = Mathf.Lerp(flashLight.intensity, intensity, Time.deltaTime * fadeSpeed);
            flashLight.spotAngle = Mathf.Lerp(flashLight.spotAngle, newSpotAngle, Time.deltaTime * fadeSpeed);
            flashLight.range = Mathf.Lerp(flashLight.range, newRange, Time.deltaTime * fadeSpeed);

            // Draw a debug line to visualize the raycast
            //Debug.DrawLine(spherecastOrigin, sphereHit.point, Color.red);
            
            // Access the GameObject that was hit
            //GameObject hitObject = sphereHit.collider.gameObject;
            //string hitObjectName = hitObject.name;
            //Debug.Log("Hit object name: " + hitObjectName);
        }
        else
        {
            // If no wall is hit, smoothly reset intensity, spot angle, and range to their maximum values
            flashLight.intensity = Mathf.Lerp(flashLight.intensity, maxIntensity, Time.deltaTime * fadeSpeed);
            flashLight.spotAngle = Mathf.Lerp(flashLight.spotAngle, maxSpotAngle, Time.deltaTime * fadeSpeed);
            flashLight.range = Mathf.Lerp(flashLight.range, maxRange, Time.deltaTime * fadeSpeed);

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