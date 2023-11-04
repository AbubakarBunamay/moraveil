using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentForce : MonoBehaviour
{
    [Header("Force/Push")]
    public float pushForceAmount = 2f; // The amount of force to apply to the player.
    public float pullForceAmount = 3f; // The amount of force to pull the player.
    public string playerTag = "Player"; // Tag of the player GameObject.

    private bool playerInsideTrigger = false; //Track whether the player is inside the trigger zone.

    // This method is called when an object enters the trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = true; // Set the flag to true if the entering object has the player tag.
        }
    }

    // This method is called when an object exits the trigger zone.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = false; // Set the flag to false if the exiting object has the player tag.
        }
    }

    // This method is called as long as an object stays within the trigger zone.
    private void OnTriggerStay(Collider other)
    {
        if (playerInsideTrigger) // Check if the player is inside the trigger zone.
        {
            CharacterController playerController = other.GetComponent<CharacterController>();

            if (playerController != null)
            {
                // Calculate a force direction from the current object to the player and normalize it.
                Vector3 forceDirection = (other.transform.position - transform.position).normalized;

                // Determine whether to apply push or pull force based on the relative positions of the player and the trigger zone.
                if (Vector3.Dot(playerController.velocity, forceDirection) > 0)
                {
                    // Apply a continuous push force to push the player in the calculated direction.
                    playerController.Move(forceDirection * pushForceAmount * Time.deltaTime);
                }
                else
                {
                    // Apply a continuous pull force to pull the player in the opposite direction.
                    playerController.Move(-forceDirection * pullForceAmount * Time.deltaTime);
                }
            }
        }
    }
}


