using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowstickController : MonoBehaviour
{
    public Light glowstickLight;  // Reference to the Light component for the glowstick.
    private bool isGlowstickOn = false;  // A boolean flag to track if the glowstick is on or off.

    public bool isInWater = false;  // Flag to track if the player is in water.

    void Start()
    {
        glowstickLight = GetComponent<Light>();  // Get the Light component of the object.
        glowstickLight.enabled = false;  // Initially, turn off the glowstick.
    }

    void Update()
    {
        // Check if the player is in water and turn on/off the glowstick accordingly.
        if (isInWater)
        {
            TurnOnGlowstick();
        }
        else
        {
            TurnOffGlowstick();
        }
    }

    // Function to turn on the glowstick.
    public void TurnOnGlowstick()
    {
        isGlowstickOn = true;
        glowstickLight.enabled = true;
    }

    // Function to turn off the glowstick.
    public void TurnOffGlowstick()
    {
        isGlowstickOn = false;
        glowstickLight.enabled = false;
    }

    // Function to set the player's state in water.
    public void SetInWater(bool inWater)
    {
        isInWater = inWater;
    }
}
