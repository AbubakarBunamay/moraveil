using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class MuralInteraction : MonoBehaviour
{
    // Reference to the sound to play
    [SerializeField]
    private AudioClip interactionSound;

    //Reference to the virutal cam
    [SerializeField]
    private CinemachineVirtualCameraBase virtualCam;

    // Reference to the player object
    [SerializeField]
    private GameObject player;

    // Flag to track player movement state
    private bool isPlayerLocked = false;
    
    // Reference to the audio source
    private AudioSource audioSource;

    // Reference to the SubtitleManager
    [SerializeField] private SubtitleManager subtitleManager;
    // Reference to the specific mural's subtitle data
    [SerializeField] private SubtitleTexts muralSubtitle; 
    
    
    private void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        
        // Find the SubtitleManager in the scene
        subtitleManager = GameObject.FindObjectOfType<SubtitleManager>();
        
        if (audioSource == null)
        {
            // Add AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Assign the interaction sound to the audio source
        audioSource.clip = interactionSound;
    }


    // Method to be called when interacting with the mural
    public void MuralInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            // Play the interaction sound
            if (interactionSound != null && audioSource != null && audioSource.isPlaying == false)
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

            // Increase virtual cam priority to be higher than main cam
            if(virtualCam)
            {
                virtualCam.m_Priority = 11;
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
        }
    }
}
