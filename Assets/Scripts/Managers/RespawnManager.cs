using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform  respawnPoint; // The point where the player will respawn.
    [SerializeField] private CharacterController player; // Reference to the player CharacterController.
    [SerializeField] private UIManager uiManager; // Reference to the UIManager script.
    [SerializeField] private GameManager gameManager; // Reference to the GameManager script.
    

    // Public method to initiate player respawn
    public void Respawn()
    {
       RespawnPlayer();
    }
    
    // Actual respawn logic for the player
    private void RespawnPlayer()
    {
        // Check if the respawn point is assigned
        if (respawnPoint != null)
        {
            // Cache the player's transform component
            Transform playerTransform = player.transform;
            
            // Update player's position and rotation to the respawn point
            player.enabled = false; // Disabling the CharacterController temporarily to directly set position
            playerTransform.position = respawnPoint.position;
            playerTransform.rotation = respawnPoint.rotation;
            player.enabled = true; // Re-enabling the CharacterController after respawning position
        
            // Restart the timer in UIManager
            if (uiManager != null)
            {
                // Reset and restarts timer
                gameManager.ResetAndStartTimer();
                // Hide the restart menu
                uiManager.RespawnUIReset();
            }
            else
            {
                // Log an error if  UIManager is not assigned
                Debug.LogError("UIManager script not assigned in the RespawnManager script.");
            }
        
            // Log a message with respawned position and rotation
            //Debug.Log("Respawned at position: " + player.transform.position + ", rotation: " + player.transform.rotation.eulerAngles);
        }
        else
        {
            // Log an error if respawn point or player is not assigned
            if (player == null)
            {
                Debug.LogError("Player CharacterController not assigned in the RespawnManager script.");
            }

            if (respawnPoint == null)
            {
                Debug.LogError("Respawn Point not set in the RespawnManager script.");
            }
        }
    }
}
