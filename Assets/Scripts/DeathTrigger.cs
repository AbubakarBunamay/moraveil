using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming your player has a script named StressManager
            StressManager stressManager = other.GetComponent<StressManager>();

            // If the StressManager script is found, increase stress to the maximum.
            if (stressManager != null)
            {
                stressManager.IncreaseStress(stressManager.maxStress);
            }

            // Notify the scene manager that the player died
            MoraveilSceneManager sceneManager = FindObjectOfType<MoraveilSceneManager>();
            if (sceneManager != null)
            {
                sceneManager.PlayerDied();
            }
        }
    }
}
