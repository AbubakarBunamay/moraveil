using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    public Light flashLight;  // Reference to a Light component for the flashlight.
    private bool isFlashlightOn = false;  // A boolean flag to track if the flashlight is on or off.
    private Transform playerCamera;  // Reference to the player's camera.

    public float raycastDistance = 10f;  // The maximum distance for raycasting.
    public Color targetColor = Color.red;  // The color to change affected objects to.
    public float colorChangeDuration = 2f;  // The duration it takes to change colors.
    private float colorChangeStartTime;  // Stores the start time of color change.


    private RaycastHit hitInfo;  // Information about what the raycast hits.
    private List<Renderer> affectedObjects = new List<Renderer>();  // A list to store affected object renderers.
    private bool isColorChanging = false;  // A flag to track if the color change is in progress.
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();  // Stores the original colors of affected objects.
    private float remainingColorChangeTime = 0f;  // The remaining time for color change.

    // Store the current coroutine for color change
    private Coroutine colorChangeCoroutine;

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
        if (SceneManager.isGamePaused)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(1))  // Check if the right mouse button is clicked.
            {
                ToggleFlashlight();  // Toggle the flashlight on and off.

                if (isFlashlightOn)
                {
                    if (!isColorChanging)
                    {
                        isColorChanging = true;  // Start color change if not already in progress.
                        colorChangeCoroutine = StartCoroutine(ChangeColors(remainingColorChangeTime));  // Start the color change coroutine.
                    }
                }
                else
                {
                    if (colorChangeCoroutine != null)
                    {
                        StopCoroutine(colorChangeCoroutine);  // Stop the color change coroutine.
                        colorChangeCoroutine = null;
                        remainingColorChangeTime = colorChangeDuration - (Time.time - colorChangeStartTime);  // Calculate the remaining time for color change.
                    }
                    isColorChanging = false;  // Stop color change.
                }
            }

            // Check if the player is underwater and the glowstick controller is available.
            if (glowstickController != null && glowstickController.isInWater)
            {
                // Turn off the flashlight and turn on the glowstick.
                isFlashlightOn = false;
                flashLight.enabled = false;
                glowstickController.TurnOnGlowstick();
            }

            if (isFlashlightOn && isColorChanging)
            {
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, raycastDistance))  // Perform a raycast from the camera.
                {
                    Renderer rend = hitInfo.collider.GetComponent<Renderer>();  // Get the renderer of the object hit by the ray.
                    if (rend != null && hitInfo.collider.CompareTag("LightObjects"))  // Check if the hit object has the "LightObjects" tag.
                    {
                        if (!affectedObjects.Contains(rend))  // Add the renderer to the list if it's not already in the list.
                        {
                            affectedObjects.Add(rend);
                            originalColors[rend] = rend.material.color;  // Store the original color of the object.
                        }
                    }
                }
            }
        }
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;  // Toggle the flashlight state.
        flashLight.enabled = isFlashlightOn;  // Turn the flashlight on or off.
        Debug.Log("Flashlight is " + (isFlashlightOn ? "on" : "off"));  // Log the flashlight state.

    }

    private IEnumerator ChangeColors(float remainingTime)
    {
        float elapsedTime = 0f;  // Initialize the elapsed time.
        colorChangeStartTime = Time.time - elapsedTime;  // Store the start time of color change.

        while (elapsedTime < colorChangeDuration)  // Continue until the color change duration is reached.
        {
            elapsedTime = Time.time - colorChangeStartTime + remainingTime;  // Calculate the elapsed time with remaining time.

            foreach (Renderer rend in affectedObjects)  // Iterate through affected objects.
            {
                rend.material.color = Color.Lerp(originalColors[rend], targetColor, elapsedTime / colorChangeDuration);  // Change the color gradually.
            }

            yield return null;  // Yielding to allow other operations in the frame.
        }
    }
}