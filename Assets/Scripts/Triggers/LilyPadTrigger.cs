using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadTrigger : MonoBehaviour
{
    public LilySoundManager lilySoundManager; // Referencing LilySoundManager 
    private AudioSource audioSource; // Audio Source of player
    public Transform playerTransform; // Reference to the player's transform
    public float moveSpeed = 2f; // The speed at which the player moves with the lilypad

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

        // Calculate the direction to move the player
        Vector3 targetPosition = transform.position; // Target position is the position of the lilypad
        targetPosition.y = playerTransform.position.y; // Maintain the player's vertical position

        // Calculate the new position for the player using MoveTowards
        Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Update the player's position
        playerTransform.position = newPosition;

    }
}
