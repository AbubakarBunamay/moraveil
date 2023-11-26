using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityManager : MonoBehaviour
{
    // Entity States
    private enum EntityState
    {
        Patrolling,
        Chasing,
        Cooldown
    }

    private NavMeshAgent navMeshAgent; // Reference to the component
    public Transform player; // To get player position
    public StressManager stressManager; // Reference to the StressManager component.
    public float entityDistancetrigger = 5.0f; // Distance to trigger stress

    public Transform[] waypoints; // List of waypoints for patrolling
    private int currentWaypointIndex = 0;

    private EntityState currentState = EntityState.Patrolling; // Current Entity Stress
    private float chaseTimer = 0.0f;  // Entity Chase Time
    public float cooldownDuration = 5.0f; // Entity Cooldown Duration 

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Find the player GameObject by its tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag(stressManager.playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Checking that there are waypoints before starting patrolling
        if (waypoints.Length > 0)
        {
            // Set the initial destination to the first waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.Log("There no waypoints set");
        }
    }

    private void Update()   
    {
        // Check what state the Entity is in and proceeds accordingly
        if (currentState == EntityState.Patrolling)
        {
            PatrollingUpdate();
        }
        else if (currentState == EntityState.Chasing)
        {
            ChasingUpdate();
        }
        else if (currentState == EntityState.Cooldown)
        {
            CooldownUpdate();
        }

        // Trigger Stress when Entity Close to player
        //EntityStress();
    }

    private void PatrollingUpdate()
    {
        if (waypoints.Length > 0)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }

        // Check if the player is within the trigger distance to activate the chase
        if (IsPlayerInTriggerDistance())
        {
            currentState = EntityState.Chasing;
            chaseTimer = 0.0f;
        }

       // EntityStress();
    }

    private bool IsPlayerInTriggerDistance()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer < entityDistancetrigger;
        }
        return false;
    }

    private void ChasingUpdate()
    {
        navMeshAgent.SetDestination(player.position);

        // Check if the player turns off the flashlight
        if (!IsPlayerFlashlightOn())
        {
            Debug.Log("Player turned off the flashlight");
            currentState = EntityState.Cooldown;
            chaseTimer = 0.0f;
        }

        chaseTimer += Time.deltaTime;
    }

    private void CooldownUpdate()
    {
        Debug.Log("Cooldown: " + chaseTimer);

        // Check if the player turns off the flashlight
        if (!IsPlayerFlashlightOn())
        {
            currentState = EntityState.Patrolling;
            return;  // Return immediately to patrolling
        }

        // Continue cooldown
        chaseTimer += Time.deltaTime;

        // Check if the cooldown duration is over
        if (chaseTimer >= cooldownDuration)
        {
            currentState = EntityState.Patrolling;
        }
    }

    //Checking if Player Flashlight is on
    private bool IsPlayerFlashlightOn()
    {

        FlashLightcontroller flashlight = player.GetComponent<FlashLightcontroller>(); // Flashlight Reference

        // Checks if flashlight Exists
        if (flashlight != null)
        {
            bool isOn = flashlight.isFlashlightOn;
            Debug.Log("Entity Flashlight detection state: " + (isOn ? "On" : "Off")); // Checks if flashlight is on or off.
            return isOn;        
        }
        else
        {
            Debug.LogError("Flashlight component not found on player.");
            return false;
        }
    }

    // Trigger entity Stress
    private void EntityStress()
    {
        if (waypoints.Length == 0)
        {
            if (stressManager != null && player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                Debug.Log("Distance to player: " + distanceToPlayer);

                if (distanceToPlayer < entityDistancetrigger)
                {
                    // Check if the player has the flashlight on
                    if (IsPlayerFlashlightOn())
                    {
                        Debug.Log("Player detected with flashlight on, initiating chase");
                        currentState = EntityState.Chasing;
                        chaseTimer = 0.0f;
                    }
                    else
                    {
                        stressManager.currentStress += stressManager.stressIncreaseRate * Time.deltaTime;
                    }
                }
            }
        }
    }

}
