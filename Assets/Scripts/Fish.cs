using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    public float rotationSpeed = 5f; // Adjust as needed

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            SetNextDestination();
        }
        else
        {
            Debug.Log("There are no waypoints set for the fish.");
        }
    }

    private void Update()
    {
        if (waypoints.Length > 0)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                SetNextDestination();
            }
        }
    }

    private void SetNextDestination()
    {
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

        // Optional: Handle rotation separately if needed
        navMeshAgent.updateRotation = false;
        Quaternion lookRotation = Quaternion.LookRotation(waypoints[currentWaypointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
