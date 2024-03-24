using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MuralInteraction : MonoBehaviour
{
    [Header("Mural Interaction")]
    [SerializeField] private CinemachineVirtualCameraBase virtualCam; //Reference to the virutal cam
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private FlashLightcontroller flashLightcontroller; // Reference to the flashlight object
    [SerializeField] private TipsPopup tipsPopup; // Reference to the TipsPopup component
    
    [Header("Mural Dialog and Subtitle")]
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager
    [SerializeField] private SubtitleTexts muralSubtitle; // Reference to the specific mural's subtitle data
    [SerializeField] private AudioClip dialogSound; // Reference to the sound to play
    
    private bool isPlayerLocked = false; // Flag to track player movement state
    private AudioSource audioSource; // Reference to the audio source
    
    private void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        
        subtitleManager = GameObject.FindObjectOfType<SubtitleManager>(); // Find the SubtitleManager in the scene
        
        flashLightcontroller = FindObjectOfType<FlashLightcontroller>(); // Get the FlashLightcontroller component
        
        if (audioSource == null)
        {
            // Add AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Assign the interaction sound to the audio source
        audioSource.clip = dialogSound;
    }


    // Method to be called when interacting with the mural
    public void MuralInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            // Play the dialogue
            if (dialogSound != null && audioSource != null && audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
            
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(muralSubtitle);
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

            // Switch to the Mural Interaction Cam
            if(virtualCam)
            {
                virtualCam.m_Priority = 11;
            }
            
            // Toggle the interaction state and update the tips pop-up text
            if (tipsPopup != null)
            {
                tipsPopup.OnPlayerInteract();
            }
            
            // Turn on the flashlight if the flashlight is turned off
            if (flashLightcontroller != null && !flashLightcontroller.isFlashlightOn)
            {
                flashLightcontroller.ToggleFlashlight();
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

            // Switch back to the player camera
            if (virtualCam)
            {
                virtualCam.m_Priority = 9;
            }
            
            // Clear the subtitle when the player stops interacting
            if (subtitleManager != null)
            {
                subtitleManager.ClearSubtitle();
            }
        }
    }
}
