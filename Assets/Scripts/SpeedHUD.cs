using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedHUD : MonoBehaviour
{
    public FPSController fpsController; // Reference to the FPSController script
    public TextMeshProUGUI speedText; // Reference to the Text UI element

    // Variable to adjust the speed increment/decrement
    public float speedChangeAmount = 1.0f;

    private void Update()
    {
        // Check if FPSController is assigned
        if (fpsController == null)
        {
            Debug.LogWarning("FPSController reference is not assigned in SpeedHUD.");
            return;
        }

        // Update the speed text with current walk and sprint speeds
        speedText.text = "Walk Speed: " + fpsController.movementSpeed +
                         "\nSprint Speed: " + fpsController.runningSpeed;

        // Check for input to increase/decrease speed
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            IncreaseWalkSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            DecreaseWalkSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Comma)) 
        {
            IncreaseSprintSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.Period)) 
        {
            DecreaseSprintSpeed();
        }
    }

    // Method to increase walk speed
    private void IncreaseWalkSpeed()
    {
        fpsController.movementSpeed += speedChangeAmount;
    }

    // Method to decrease walk speed
    private void DecreaseWalkSpeed()
    {
        fpsController.movementSpeed -= speedChangeAmount;
        // Ensuring walk speed doesn't go below zero
        fpsController.movementSpeed = Mathf.Max(0f, fpsController.movementSpeed);
    }

    // Method to increase sprint speed
    private void IncreaseSprintSpeed()
    {
        fpsController.runningSpeed += speedChangeAmount;
        fpsController.runningSpeed = Mathf.Max(0f, fpsController.runningSpeed);
        fpsController.originalRunningSpeed = fpsController.runningSpeed;
    }

    // Method to decrease sprint speed
    private void DecreaseSprintSpeed()
    {
        fpsController.runningSpeed -= speedChangeAmount;
        // Ensuring sprint speed doesn't go below zero
        fpsController.runningSpeed = Mathf.Max(0f, fpsController.runningSpeed);
        fpsController.originalRunningSpeed = fpsController.runningSpeed;
    }
}
