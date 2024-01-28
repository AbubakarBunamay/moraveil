using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public RespawnManager respawnManager; // Respawn Manager Reference
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger area has a specific tag (in this case, "Player").
        if (other.CompareTag("Player"))
        {
            StressManager stressManager = other.GetComponent<StressManager>();

            // If the StressManager script is found, increase stress to the maximum.
            if (stressManager != null)
            {
                stressManager.IncreaseStress(stressManager.maxStress);
            }

            // Notify the scene manager that the player died
            // UIManager sceneManager = FindObjectOfType<UIManager>();
            // if (sceneManager != null)
            // {
            //     sceneManager.PlayerDied();
            // }
            
            // Use the RespawnManager to respawn the player
            if (respawnManager != null)
            {
                respawnManager.Respawn();
            }
        }
    }
}
