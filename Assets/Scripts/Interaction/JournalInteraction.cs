using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInteraction : MonoBehaviour
{
    private Animator animator; // Reference to the Animator
    [SerializeField] private CinemachineVirtualCameraBase virtualCam; //Reference to the virutal cam
    private AudioSource audioSource; // Reference to the audio source
    [SerializeField] private GameObject player; // Reference to the player object

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

        // Increase virtual cam priority to be higher than main cam
        if (virtualCam)
        {
            virtualCam.m_Priority = 11;
        }

    
        else
        {
            // Stop the audio if it's playing
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

        // Unlock the player
        if (player != null)
        {
            // Re-enable player movement
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = true;
            }
        }

        // Revert virtual cam priority to be lower than main cam
        if (virtualCam)
        {
            virtualCam.m_Priority = 9;
        }
                
        }
           
    }



}
