using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuralInteraction : MonoBehaviour
{
    // Reference to the sound to play
    [SerializeField]
    private AudioClip interactionSound;

    // Reference to the player object
    [SerializeField]
    private GameObject player;

    // Flag to track player movement state
    private bool isPlayerLocked = false;
    
    // Reference to the audio source
    private AudioSource audioSource;
    
    private void Start()
    {
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


    // Method to be called when interacting with the mural
    public void MuralInteract()
    {
        // Toggle player movement lock state
        isPlayerLocked = !isPlayerLocked;

        if (isPlayerLocked)
        {
            // Play the interaction sound
            if (interactionSound != null && audioSource != null)
            {
                audioSource.Play();
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
        }
    }
}
