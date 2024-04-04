using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JournalInteraction : MonoBehaviour
{
    
    [Header("Journal Interaction")]
    [SerializeField] private CinemachineVirtualCameraBase virtualCam; //Reference to the virutal cam
    [SerializeField] private GameObject player; // Reference to the player object  
    [SerializeField] private TipsPopup tipsPopup; // Reference to the TipsPopup component
    [SerializeField] private FlashLightcontroller flashLightcontroller; // Reference to the flashlight object

    [Header("Journal Dialogue & Subtitle")]
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager
    [SerializeField] private SubtitleTexts journalSubtitle; // Reference to the specific polaroid subtitle data
    [SerializeField] private AudioClip dialogueSound; // Reference to the sound to play
    
    
    private Animator animator; // Reference to the Animator
    private bool isPlayerLocked = false; // Flag to track player movement state
    private AudioSource audioSource; // Reference to the audio source

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
        
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        flashLightcontroller = FindObjectOfType<FlashLightcontroller>(); // Get the FlashLightcontroller component

        // Assign the interaction sound to the audio source
        audioSource.clip = dialogueSound;
    }
    public void JournalInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            audioSource.Play(); // Journal Dialogue Play
            // Trigger Journal Interaction Animation
            animator.SetTrigger("journal_Interaction");

            // Switch to the Journal Interaction Cam
            if (virtualCam)
            {
                virtualCam.m_Priority = 11;
            }

            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(journalSubtitle);
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
            
            // Turn off the flashlight if the flashlight is turned on
            if (flashLightcontroller != null && flashLightcontroller.isFlashlightOn)
            {
                flashLightcontroller.ToggleOffFlashlight();
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

            // Revert to the player camera
            if (virtualCam)
            {
                virtualCam.m_Priority = 9;
            }
            
            // Toggle the interaction state and remove the tips pop-up 
            if (tipsPopup != null)
            {
                tipsPopup.DeactivateAndClearText();
            }

            // Deactivate the Journal GameObject
            gameObject.SetActive(false);
            
            // Clear the subtitle when the player stops interacting
            if (subtitleManager != null)
            {
                subtitleManager.ClearSubtitle();
            }
        }

    }



}
