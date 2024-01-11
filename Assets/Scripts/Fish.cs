using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;         // Reference to the NavMeshAgent component
    public Transform[] waypoints;              // Array of waypoints for the fish to follow
    private int currentWaypointIndex = 0;      // Index to keep track of the current waypoint

    public float rotationSpeed = 5f;           // Rotation speed for the fish

    private void Start()
    {
        // Get the NavMeshAgent component attached to the same GameObject
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Check if there are waypoints provided
        if (waypoints.Length > 0)
        {
            // If waypoints exist, set the initial destination
            SetNextDestination();
        }
        else
        {
            // Log a warning if no waypoints are set
            Debug.Log("There are no waypoints set for the fish.");
        }
    }

    private void Update()
    {
        // Check if waypoints exist
        if (waypoints.Length > 0)
        {
            // Check if the fish is close to the current waypoint
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                // Move to the next waypoint in a loop
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

                // Set the next destination based on the updated waypoint index
                SetNextDestination();
            }
        }
    }

    private void SetNextDestination()
    {
        // Set the destination for the NavMeshAgent to the position of the current waypoint
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        // Disable automatic rotation by the NavMeshAgent
        navMeshAgent.updateRotation = false;

        // Calculate the rotation needed to face the current waypoint
        Quaternion lookRotation = Quaternion.LookRotation(waypoints[currentWaypointIndex].position - transform.position);

        // Smoothly rotate the fish towards the waypoint using Slerp (Spherical Linear Interpolation)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
