using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityManager : MonoBehaviour
{
    // Singleton instance
    public static EntityManager Instance { get; private set; }

    public Transform player; // To get player position
    public StressManager stressManager; // Reference to the StressManager component.
   
    private List<Entity> entities = new List<Entity>();     // List of the entities
    private Transform[][] entityWaypoints; // Array of entities & their waypoints

    private void Awake()
    {
        // Ensure only one instance of EntityManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Register an entity with the manager
    public void RegisterEntity(Entity entity)
    {
        entities.Add(entity);
    }

    // Unregister an entity from the manager
    public void UnregisterEntity(Entity entity)
    {
        entities.Remove(entity);
    }

    // Get all registered entities
    public Entity[] GetAllEntities()
    {
        return entities.ToArray();
    }
    
    
    private void Start()
    {
        // Find the player GameObject by its tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

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

    // Method to initialize entities with waypoints
    private void InitializeEntities()
    {
        // Loop through each entity waypoint and corresponding entity in the list
        for (int i = 0; i < entityWaypoints.Length && i < entities.Count; i++)
        {
            // Get the current entity from the list
            Entity entity = entities[i];

            // Check if both player and stressManager are assigned before proceeding with initialization
            if (player != null && stressManager != null)
            {
                // Initialize the entity with specific waypoints
                entity.Initialize(entityWaypoints[i]);
            }
            else
            {
                // Log an error if either the player or stressManager is not assigned
                Debug.LogError("Player or stressManager not assigned in the EntityManager!");
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
    
    // Method to Update Entity States
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
