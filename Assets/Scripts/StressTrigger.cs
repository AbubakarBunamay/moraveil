using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTrigger : MonoBehaviour
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

        // Increase stress if the player is inside the trigger zone.
        if (playerInsideTrigger)
        {
            // Update stress, trigger stress effects, and update UI.
            stressManager.IncreaseStressTrigger();
        }
        else
        {
            // Decrease stress continuously if the player is outside the trigger zone.
            stressManager.DecreaseStressTrigger();
        }

        // Always update stress effects and UI.
        stressManager.StressEffects();
        stressManager.ResetStressEffects();

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
