using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingStressTrigger : MonoBehaviour
{
    public StressManager stressManager;
    private bool playerInsideTrigger = false; // By default player not inside trigger

    // Start is called before the first frame update
    void Start()
    {
        stressManager = FindObjectOfType<StressManager>();
    }

    public void Update()
    {
        if (playerInsideTrigger)
        {
            // Increase stress only if the player is stressed.
            if (stressManager.currentStress > 0)
            {
                stressManager.IncreaseSafezoneStress();
                Debug.Log("Increasing Hiding stress: " + stressManager.currentStress);
            }
            else
            {
                // Decrease stress if the player is not stressed.
                stressManager.DecreaseStressTrigger();
                Debug.Log("Decreasing if the player is not stressed: " + stressManager.currentStress);
            }

            // Trigger stress effects and update UI.
            stressManager.StressEffects();
        }
        else
        {
            // Reset stress effects and continuously decrease stress if the player is outside the trigger zone.
            stressManager.ResetStressEffects();
            stressManager.DecreaseStressTrigger();
            Debug.Log("Decreasing stress: " + stressManager.currentStress);
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if (stressManager != null && other.CompareTag(stressManager.playerTag))
        {
            playerInsideTrigger = true;
        }
    }

    // OnTriggerExit is called when the Collider other exits the trigger
    void OnTriggerExit(Collider other)
    {
        if (stressManager != null && other.CompareTag(stressManager.playerTag))
        {
            playerInsideTrigger = false;
        }
    }

}
