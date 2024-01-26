using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityManager : MonoBehaviour
{
   public Transform player; // To get player position
    public StressManager stressManager; // Reference to the StressManager component.
    
    // List of the entities
    private List<Entity> entities = new List<Entity>();
    
    public Transform[][] entityWaypoints; // Array of entities & their waypoints

    private void Start()
    {
        // Find the player GameObject by its tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag(stressManager.playerTag);

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
        
        // Create and initialize enemy entities
        InitializeEntities();
    }

    private void InitializeEntities()
    {
        for (int i = 0; i < entityWaypoints.Length && i < entities.Count; i++)
        {
            Entity entity = entities[i];

            // Checking if player and stressManager are assigned before calling Initialize
            if (player != null && stressManager != null)
            {
                // Initialize the entity with the player, stressManager, and specific waypoints
                entity.Initialize(player, stressManager, entityWaypoints[i]);
            }
            else
            {
                Debug.LogError("Player or stressManager not assigned!");
            }
        }
    }

    private void Update()
    {
        // Update enemy entities
        foreach (Entity enemyEntity in entities)
        {
            enemyEntity.Update();
        }
    }

    public void UpdateEntityState(Entity entity)
    {
        // Call the corresponding entity state update based on the entity's current state
        switch (entity.currentState)
        {
            case Entity.EntityState.Patrolling:
                entity.PatrollingUpdate();
                break;

            case Entity.EntityState.Chasing:
                entity.ChasingUpdate();
                break;

            case Entity.EntityState.Cooldown:
                entity.CooldownUpdate();
                break;
        }
    }
}
