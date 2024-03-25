using System.Collections;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToFall; // Array to hold objects that will fall
    [SerializeField] private float minDelay = 1f, maxDelay = 3f; // Minimum delay before next rock falls & Maximum delay before next rock falls

    private Vector3[] originalPositions; // Array to hold original positions of falling objects
    
   private ParticleSystem[] rockHitParticles; // Array to hold particle systems of rocks
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player has entered the trigger zone
        {
            // Start the coroutine to make rocks fall periodically
            StartCoroutine(FallRocksPeriodically());
        }
    }

    private void Start()
    {
        // Store the original positions of the falling objects
        originalPositions = new Vector3[objectsToFall.Length];
        
        // Disable gravity and set kinematic to true for each rock
        foreach (GameObject rock in objectsToFall)
        {
            Rigidbody[] childRbs = rock.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in childRbs)
            {
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
            }
        }
        
        // Initialize an array to hold particle systems
        rockHitParticles = new ParticleSystem[objectsToFall.Length];

        // Getting references to the particle systems of each rock
        for (int i = 0; i < objectsToFall.Length; i++)
        {
            rockHitParticles[i] = objectsToFall[i].GetComponentInChildren<ParticleSystem>();
        }
    }

    // Coroutine to make rocks fall periodically
    private IEnumerator FallRocksPeriodically()
    {
        // Track the number of rocks that have fallen
        int rocksFallen = 0;
        
        // Repeat Until all rocks have fallen
        while (rocksFallen < objectsToFall.Length)
        {
            // Make one random rock fall
            int randomRockIndex = Random.Range(0, objectsToFall.Length);
            GameObject rockToFall = objectsToFall[randomRockIndex];

            // Enable Rigidbody and gravity for both the parent and its children
            Rigidbody[] rbs = rockToFall.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }
            
            // Activate the particle system of the falling rock and deactivate others
            // Simply works as only play when rock is falling and stop when next rock falls
            for (int i = 0; i < rockHitParticles.Length; i++)
            {
                if (i == randomRockIndex && rockHitParticles[i] != null)
                {
                    rockHitParticles[i].Play();
                }
                else if (rockHitParticles[i] != null)
                {
                    rockHitParticles[i].Stop();
                }
            }
            
            // Increment the count of fallen rocks
            rocksFallen++;
            
            // Wait for a random delay before the next fall
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
