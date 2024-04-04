using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PolaroidInteraction : MonoBehaviour
{
    [Header("Polaroid Interaction")]
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private CinemachineVirtualCameraBase virtualCam; //Reference to the virutal cam
    [SerializeField] private TipsPopup tipsPopup; // Reference to the TipsPopup component

    [Header("Polaroid Dialogue & Subtitle")]
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager
    [SerializeField] private SubtitleTexts polaroidSubtitle; // Reference to the specific polaroid subtitle data
    [SerializeField] private AudioClip dialogueSound; // Reference to the sound to play

    private bool isPlayerLocked = false; // Flag to track player movement state
    private AudioSource audioSource; // Reference to the audio source
    private Animator animator; // Reference to the animator

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
        
        // Assign the dialogue sound to the audio source
        audioSource.clip = dialogueSound;
        
    }
    
    // Method to be called when interacting with the polaroid
    public void PolaroidInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            // Play the interaction sound
            if (dialogueSound != null && audioSource != null)
            {
                audioSource.Play();
                // Trigger Polaroid Interaction Animation
                animator.SetTrigger("polaroidInteraction");
            }
            
            // Switch to the polaroid interaction camera
            if (virtualCam)
            {
                virtualCam.m_Priority = 11;
            }
            
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(polaroidSubtitle);
            }
            
            // Toggle the interaction state and update the tips pop-up text
            if (tipsPopup != null)
            {
                tipsPopup.OnPlayerInteract();
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
            
            // Revert back to the player camera
            if (virtualCam)
            {
                virtualCam.m_Priority = 9;
            }
            // Toggle the interaction state and remove the tips pop-up 
            if (tipsPopup != null)
            {
                tipsPopup.DeactivateAndClearText();
            }
            
            // Deactivate the polaroid GameObject
            gameObject.SetActive(false);
            
            // Clear the subtitle when the player stops interacting
            if (subtitleManager != null)
            {
                subtitleManager.ClearSubtitle();
            }
        }
    }
            
}
