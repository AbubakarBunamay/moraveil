using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityManager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player; 
    public StressManager stressManager; // Reference to the StressManager component.


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
        if (player != null)
        {
            // Set the destination to follow the player
            navMeshAgent.SetDestination(player.position);
        }

        // Check stress-related conditions and call the stress manager's functions.
        if (stressManager != null)
        {
            // Example: Call the TriggerStress and IncreaseStress functions from StressManager.
            stressManager.TriggerStress();
            stressManager.IncreaseStress();
        }
    }

    private void EntityStress()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if(distanceToPlayer < 5.0f)
            {
                stressManager.currentStress += stressManager.stressIncreaseRate * Time.deltaTime;
            }
        }
    }

}
