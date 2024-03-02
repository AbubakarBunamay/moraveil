using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInteraction : MonoBehaviour
{
    private Animator animator; // Reference to the Animator

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();

        // Check if animator is not null
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Journal!");
        }
    }
    public void JournalInteract()
    {
        // Trigger Journal Interaction Animation
        animator.SetTrigger("journal_Interaction");
    }
}
