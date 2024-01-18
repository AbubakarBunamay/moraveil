using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public Transform  respawnPoint; // The point where the player will respawn.
    public GameObject player; // Reference to the player GameObject.
    
    // Public method to initiate player respawn
    public void Respawn()
    {
       RespawnPlayer();
    }
    
    // Actual respawn logic for the player
    public void RespawnPlayer()
    {
        // Check if the respawn point is assigned
        if (respawnPoint != null)
        {
            // Update player's position and rotation to the respawn point
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;
            
            // Log a debug message with respawned position and rotation
            Debug.Log("respawned at position: " + transform.position + ", rotation: " + transform.rotation.eulerAngles);

        }
        else
        {
            // Error handling: Log an error if respawn point or player is not assigned
            if (player == null)
            {
                Debug.LogError("Player GameObject not assigned in the RespawnManager script.");
            }

            if (respawnPoint == null)
            {
                Debug.LogError("Respawn Point not set in the RespawnManager script.");
            }
        }
    }
}
