using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    //Entity States
    public enum EntityState
    {
        Patrolling,
        Chasing,
        Cooldown
    }
    
    private EntityManager entityManager; // Reference to the EntityManager
    
    private NavMeshAgent navMeshAgent;     // Reference to the NavMeshAgent component
    private Transform player; // Reference to the player's transform
    private StressManager stressManager;     // Reference to the StressManager
    
    [SerializeField] private float entityDistancetrigger = 5.0f;     // Trigger distance for activating the chase state
    [SerializeField] private Transform[] waypoints; // Array of waypoints for patrolling
    private int currentWaypointIndex = 0; // Index of the current waypoint in the array
    
    // Entity States 
    public EntityState currentState = EntityState.Patrolling; // Current state of the entity
    private float chaseTimer = 0.0f; // Timer for the chase state
    private float cooldownDuration ; // Cooldown duration after chasing
    private float distanceToPlayer; // Distance to the player
    [SerializeField] private  float rotationSpeed = 5.0f; // Rotation speed of the entity
    
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
            Debug.Log("EntityManager not found!");
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
            Debug.Log("There no waypoints set on" + gameObject.name); // If no waypoints have been add (Error Handler)
        }
        
        // Register this entity with the EntityManager
        EntityManager.Instance.RegisterEntity(this);
        SetReferencesFromManager(); //Setting References
    }
    
    private void OnDestroy()
    {
        // Unregister this entity from the EntityManager
        EntityManager.Instance.UnregisterEntity(this);
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if the EntityManager is not null and update the entity state
        if (entityManager != null)
        {
            ImplementDefaultEntityLogic();
        }
        
        // Check if the player is stunned, then proceed to patrol
        if (stressManager != null && stressManager.IsPlayerStunned())
        {
            currentState = EntityState.Patrolling;
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
            
            // Check if the player is within the trigger distance to increase stress
            if (IsPlayerWithinStressTriggerDistance())
            {
                // Increase stress as the player is close
                float stressIncrease = entityStressIncreaseRate * Time.deltaTime;
                stressManager.IncreaseStress(stressIncrease);
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
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the trigger distance
            if (distanceToPlayer < entityDistancetrigger)
            {
                // Rotate the entity to look at the player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


                // Check if the player has the flashlight on
                if (IsPlayerFlashlightOn())
                {
                    return true;
                }
            }
        }

        // Return false by default
        return false;
    }
    
    // Check if the player is within the trigger distance
    private bool IsPlayerWithinStressTriggerDistance()
    {
        // Check if the player is not null
        if (player != null)
        {
            // Calculate the distance to the player
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if the player is within the trigger distance
            return distanceToPlayer < entityDistancetrigger;
        }

        return false;
    }
    
    // Calculate stress increase based on distance
    private float CalculateStressIncrease(float distance, float maxstress)
    {
        float stressChangeThreshold = entityDistancetrigger;
        float stressFactor = Mathf.Clamp01((stressChangeThreshold - distance) / stressChangeThreshold);
        return Mathf.Lerp(-entityDistancetrigger, entityStressIncreaseRate, stressFactor) * maxstress;
    }
    
    public void OnPlayerStunned()
    {
        // Change the entity's state to patrolling
        currentState = EntityState.Patrolling;
    }
    // Update logic for the Chasing state
    public void ChasingUpdate()
    {
        // Set the destination of the NavMeshAgent to the player's position
        navMeshAgent.SetDestination(player.position);
        
        // Check if the player is within the trigger distance to increase stress
        if (IsPlayerWithinStressTriggerDistance())
        {
            // Increase stress as the player is close
            float stressIncrease = entityStressIncreaseRate * Time.deltaTime;
            stressManager.IncreaseStress(stressIncrease);
        }
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
        
        // Decrease stress during cooldown
        stressManager.DecreaseStress(entityStressDecreaseRate * Time.deltaTime);

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
                //Debug.Log("Entity Flashlight detection state: " + (isOn ? "On" : "Off"));
                return isOn;
            }
            else
            {
                // Log an error if the flashlight component is not found on the player
                Debug.LogError("Flashlight component not found on player from Entity Script.", this);
                return false;
            }
        }
        else
        {
            // Log an error if the player object is null
            Debug.LogError("Flashlight in Player object is null from Entity Script.", this);
            return false;
        }
    }
    
    public void Initialize(Transform[] assignedWaypoints)
    {
        // Check if there are waypoints assigned before setting the initial destination
        if (assignedWaypoints.Length > 0)
        {
            waypoints = assignedWaypoints;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            // Log a message if no waypoints have been set
            Debug.LogError("No waypoints set on Entities", this);
        }
    }

    private void SetReferencesFromManager()
    {
        // Check if the EntityManager is not null
        if (entityManager != null)
        {
            // Set player and stressManager references from EntityManager
            player = entityManager.player;
            stressManager = entityManager.stressManager;

            Debug.Log("Player and StressManager references set successfully.");
        }
        else
        {
            // Log an error if the EntityManager is not found
            Debug.LogError("EntityManager not found!");
        }
    }
    
}
