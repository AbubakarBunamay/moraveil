using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolInteract : Interactable
{
    private bool isCameraLocked = false;

    public override void Interact()
    {
        base.Interact();

        // Toggle camera lock
        isCameraLocked = IsInteracting();

        if (isCameraLocked)
        {
            SetCameraLock();
        }
        else
        {
            ReleaseCameraLock();
        }
    }

    private void SetCameraLock()
    {
        FPSController lookScript = Camera.main.GetComponentInParent<FPSController>();
        if (lookScript != null)
        {
            lookScript.enabled = false;
        }

        Debug.Log("Camera locked on " + gameObject.name);
    }

    private void ReleaseCameraLock()
    {
        FPSController lookScript = Camera.main.GetComponentInParent<FPSController>();
        if (lookScript != null)
        {
            lookScript.enabled = true;
        }

        Debug.Log("Camera released from " + gameObject.name);
    }
}
