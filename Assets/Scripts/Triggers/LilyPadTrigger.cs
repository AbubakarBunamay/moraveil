using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadTrigger : MonoBehaviour
{
    [SerializeField] private LilySoundManager lilySoundManager; // Referencing LilySoundManager 
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private float moveSpeed = 2f; // The speed at which the player moves with the lilypad
    
    private AudioSource audioSource; // Audio Source of player
    private Vector3 previousLilypadPosition; // Previous position of the lilypad

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        lilySoundManager = FindObjectOfType<LilySoundManager>();
        // Find the player GameObject in the scene by tag
        GameObject playerObject = GameObject.FindWithTag("Player");

        // Check if the player GameObject was found
        if (playerObject != null)
        {
            // Get the player's transform component
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player GameObject not found in the scene!");
        }
        
        // Initialize previous lilypad position
        previousLilypadPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger area has a specific tag (in this case, "Player").
        if (other.CompareTag("Player"))
        {
            // Play a random sound from the array
            int randomIndex = Random.Range(0, lilySoundManager.lilySounds.Length);
            audioSource.PlayOneShot(lilySoundManager.lilySounds[randomIndex]);
        }
    }
    
    // Method to synchronize player movement with lilypad
    public void MovePlayerWithLilypad()
    {
        // Calculate the movement of the lilypad
        Vector3 lilypadMovement = transform.position - previousLilypadPosition;
        
        // Move the player along with the lilypad's movement, scaled by moveSpeed
        playerTransform.position += lilypadMovement * moveSpeed;

        // Update the previous lilypad position
        previousLilypadPosition = transform.position;
    }
}
