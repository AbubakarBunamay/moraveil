using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent; // Reference to the component
    public Transform player; // To get player position
    public StressManager stressManager; // Reference to the StressManager component.
    public float entityDistancetrigger = 5.0f; // Distance to trigger stress

    public Transform[] waypoints; // List of waypoints for patrolling
    private int currentWaypointIndex = 0;

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
        // Check if there are waypoints
        if (waypoints.Length > 0)
        {
            Debug.Log("Destination: " + navMeshAgent.destination);

            // If the entity is close to the current waypoint, set the next waypoint as the destination
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }

        // Check if an entity is close to the player and trigger stress
        EntityStress();
    }

    private void EntityStress()
    {
        if (waypoints.Length == 0)
        {
            // Check if there is a player
            if (stressManager != null && player != null)
            {
                // Calculate distance to the player
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // Increase stress when getting close to the player
                if (distanceToPlayer < entityDistancetrigger)
                {
                    stressManager.currentStress += stressManager.stressIncreaseRate * Time.deltaTime;
                }
            }
        }
    }

}
