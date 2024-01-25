using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public enum EntityState
    {
        Patrolling,
        Chasing,
        Cooldown
    }
    
    private EntityManager entityManager; // Reference to the EntityManager
    
    private NavMeshAgent navMeshAgent;     // Reference to the NavMeshAgent component
    public Transform player; // Reference to the player's transform
    public StressManager stressManager;     // Reference to the StressManager
    public float entityDistancetrigger = 5.0f;     // Trigger distance for activating the chase state
    
    public Transform[] waypoints; // Array of waypoints for patrolling
    private int currentWaypointIndex = 0; // Index of the current waypoint in the array

    public EntityState currentState = EntityState.Patrolling; // Current state of the entity
    public float chaseTimer = 0.0f; // Timer for the chase state
    public float cooldownDuration ; // Cooldown duration after chasing
    private float distanceToPlayer; // Distance to the player
    
    // Stress Variables
    public float entityStressIncreaseRate; // Separate stress increase rate for this entity
    public float entityStressDecreaseRate; // Separate stress increase rate for this entity

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component attached to the entity
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Find the EntityManager in the scene
        entityManager = FindObjectOfType<EntityManager>();

        // Log an error if the EntityManager is not found
        if (entityManager == null)
        {
            Debug.LogError("EntityManager not found!");
            return;
        }
        
        // Checking that there are waypoints before starting patrolling
        if (waypoints.Length > 0)
        {
            // Set the initial destination to the first waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.Log("There no waypoints set"); // If no waypoints have been add (Error Handler)
        }
        
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if the EntityManager is not null and update the entity state
        if (entityManager != null)
        {
            ImplementDefaultEntityLogic();
        }
    }
    
    // Update the entity state using the EntityManager
    private void ImplementDefaultEntityLogic()
    {
        // Find the EntityManager in the scene
        entityManager = FindObjectOfType<EntityManager>();

        // Check if the EntityManager is not null and update the entity state
        if (entityManager != null)
        {
            entityManager.UpdateEntityState(this);
        }
    }
    
    // Update logic for the Patrolling state
    public void PatrollingUpdate()
    {
        // Check if the current state is Patrolling
        if (currentState == EntityState.Patrolling)
        {
            // Check if there are waypoints
            if (waypoints.Length > 0)
            {
                // Check if the entity has reached the current waypoint
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    // Move to the next waypoint in a manner
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
        }
    }
    
    // Check if the player is within the trigger distance
    private bool IsPlayerInTriggerDistance()
    {
        // Check if the player is not null and the current state is Patrolling
        if (player != null && currentState == EntityState.Patrolling)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the trigger distance
            if (distanceToPlayer < entityDistancetrigger)
            {
                // Check if the player has the flashlight on
                if (IsPlayerFlashlightOn())
                {
                    currentState = EntityState.Chasing;
                    chaseTimer = 0.0f;
                }
                else
                {
                    // Increase stress only if it won't exceed the maximum stress.
                    float stressIncrease = entityStressIncreaseRate * Time.deltaTime;
                    float remainingStressSpace = stressManager.maxStress - stressManager.currentStress;

                    // Check if there is remaining stress space
                    if (remainingStressSpace > 0)
                    {
                        // Increase stress and trigger stress effects
                        stressManager.IncreaseStress(stressIncrease);
                    }
                }
            }
        }

        // Return false by default
        return false;
    }
    
    // Update logic for the Chasing state
    public void ChasingUpdate()
    {
        // Set the destination of the NavMeshAgent to the player's position
        navMeshAgent.SetDestination(player.position);

        // Check if the player turns off the flashlight
        if (!IsPlayerFlashlightOn())
        {
            Debug.Log("Player turned off the flashlight");
            currentState = EntityState.Cooldown;
            chaseTimer = 0.0f;
        }

        // Increment the chase timer
        chaseTimer += Time.deltaTime;
    }
    
    // Update logic for the Cooldown state
    public void CooldownUpdate()
    {
        Debug.Log("Cooldown: " + chaseTimer);

        // Continue cooldown
        chaseTimer += Time.deltaTime;

        // Check if the player turns off the flashlight
        if (!IsPlayerFlashlightOn())
        {
            Debug.Log("Player turned off the flashlight");
            currentState = EntityState.Patrolling;
            // Reset the chaseTimer when transitioning to patrolling
            chaseTimer = 0.0f;
            return;
        }

        // Check if the cooldown duration is over
        if (chaseTimer >= cooldownDuration)
        {
            currentState = EntityState.Patrolling;
            // Reset the chaseTimer when transitioning to patrolling
            chaseTimer = 0.0f;
        }
    }
    
    // Check if the player's flashlight is on
    private bool IsPlayerFlashlightOn()
    {
        // Check if the player is not null
        if (player != null)
        {
            // Get the FlashLightcontroller component attached to the player
            FlashLightcontroller flashlight = player.GetComponentInChildren<FlashLightcontroller>();

            // Check if the flashlight component is not null
            if (flashlight != null)
            {
                // Get the state of the flashlight
                bool isOn = flashlight.isFlashlightOn;
                Debug.Log("Entity Flashlight detection state: " + (isOn ? "On" : "Off"));
                return isOn;
            }
            else
            {
                // Log an error if the flashlight component is not found on the player
                Debug.LogError("Flashlight component not found on player.");
                return false;
            }
        }
        else
        {
            // Log an error if the player object is null
            Debug.LogError("Player object is null.");
            return false;
        }
    }
    
    // This method is called by EntityManager to set initial values
    public void Initialize(Transform player, StressManager stressManager, Transform[] assignedWaypoints)
    {
        // Set the player and stressManager references
        this.player = player;
        this.stressManager = stressManager;

        // Check if there are waypoints assigned before setting the initial destination
        if (assignedWaypoints.Length > 0)
        {
            waypoints = assignedWaypoints;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            // Log a message if no waypoints have been set
            Debug.Log("No waypoints set");
        }
    }
    
}
