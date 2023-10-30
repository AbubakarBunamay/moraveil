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


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Find the player GameObject by its tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag(stressManager.playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        // Check player
        if (player != null)
        {
            // Set the destination to follow the player
            navMeshAgent.SetDestination(player.position);
        }

        // Check stress-related conditions and call the stress manager's functions.
        if (stressManager != null)
        {
            // Calling TriggerStress and IncreaseStress functions from StressManager.
            stressManager.TriggerStress();
            stressManager.IncreaseStress();
        }
    }

    private void EntityStress()
    {

        // Check player
        if (player != null)
        {
            //Calculate distance to player 
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Increase stress when getting close to the player 
            if(distanceToPlayer < entityDistancetrigger)
            {
                stressManager.currentStress += stressManager.stressIncreaseRate * Time.deltaTime;
            }
        }
    }

}
