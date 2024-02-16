using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidInteraction : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();

        // Check if animator is not null
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Polaroid!");
        }
    }

    public void PolaroidInteract()
    {
        // Trigger Polaroid Interaction Animation
        animator.SetTrigger("polaroidInteraction");
            
    }
}
