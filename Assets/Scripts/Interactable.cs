using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool isInteracting = false;

    public virtual void Interact()
    {
        isInteracting = !isInteracting;
    }

    public bool IsInteracting()
    {
        return isInteracting;
    }
}
