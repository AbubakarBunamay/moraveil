using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool isInteracting = false; // bool to check if player isInteracting

    // Interact Method
    public virtual void Interact()
    {
        isInteracting = !isInteracting;
    }

    // IsInteracting method check  
    public bool IsInteracting()
    {
        return isInteracting;
    }
}
