using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteract : Interactable
{
    private bool isCameraLocked = false; // Flag to track whether the camera is currently locked.
   
    // Override the Interact method from the base class.
    public override void Interact()
    {
        base.Interact(); // Call the Interact method of the base class.

        // Toggle camera lock based on the result of IsInteracting().
        isCameraLocked = IsInteracting();

        // If the camera is locked, call SetCameraLock(); otherwise, call ReleaseCameraLock().
        if (isCameraLocked)
        {
            SetCameraLock();
        }
        else
        {
            ReleaseCameraLock();
        }

    }

    // Method to set the camera lock.
    private void SetCameraLock()
    {
        // Get the FPSController component from the main camera's parent.
        FPSController lookScript = Camera.main.GetComponentInParent<FPSController>();

        // If the FPSController component is found, disable its script.
        if (lookScript != null)
        {
            lookScript.enabled = false;
        }

        // Log a message indicating that the camera is locked on the current symbol.
        Debug.Log("Camera locked on " + gameObject.name);
    }

    // Method to release the camera lock.
    private void ReleaseCameraLock()
    {
        // Get the FPSController component from the main camera's parent.
        FPSController lookScript = Camera.main.GetComponentInParent<FPSController>();

        // If the FPSController component is found, enable its script.
        if (lookScript != null)
        {
            lookScript.enabled = true;
        }

        // Log a message indicating that the camera is released from the current symbol.
        Debug.Log("Camera released from " + gameObject.name);
    }
}
