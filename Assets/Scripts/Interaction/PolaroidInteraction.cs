using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PolaroidInteraction : MonoBehaviour
{
    
    [SerializeField] private AudioClip interactionSound; // Reference to the sound to play
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private CinemachineVirtualCameraBase virtualCam; //Reference to the virutal cam

    private bool isPlayerLocked = false; // Flag to track player movement state
    private AudioSource audioSource; // Reference to the audio source
    private Animator animator; // Reference to the animator
    
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager
    [SerializeField] private SubtitleTexts polaroidSubtitle; // Reference to the specific polaroid subtitle data

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
        
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Assign the interaction sound to the audio source
        audioSource.clip = interactionSound;
        
    }

    public void PolaroidInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            // Play the interaction sound
            if (interactionSound != null && audioSource != null)
            {
                audioSource.Play();
                // Trigger Polaroid Interaction Animation
                animator.SetTrigger("polaroidInteraction");
            }
            
            // Increase virtual cam priority to be higher than main cam
            if (virtualCam)
            {
                virtualCam.m_Priority = 11;
            }
            
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(polaroidSubtitle);
            }

            // Lock the player
            if (player != null)
            {
                // Disable player movement 
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }
            }
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
            
            // Deactivate the polaroid GameObject
            gameObject.SetActive(false);
        }
    }
            
    }
