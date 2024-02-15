using System.Collections;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsToFall; // Array to hold objects that will fall
    [SerializeField]
    private float minDelay = 1f, maxDelay = 3f; // Minimum delay before next rock falls & Maximum delay before next rock falls

    private Vector3[] originalPositions; // Array to hold original positions of falling objects

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
        for (int i = 0; i < objectsToFall.Length; i++)
        {
            originalPositions[i] = objectsToFall[i].transform.position;
        }
    }

    // Coroutine to make rocks fall periodically
    private IEnumerator FallRocksPeriodically()
    {
        while (true)
        {
            // Make one random rock fall
            int randomRockIndex = Random.Range(0, objectsToFall.Length);
            GameObject rockToFall = objectsToFall[randomRockIndex];

            Rigidbody rb = rockToFall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Enable Rigidbody and gravity
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            
            // Wait for a random delay before the next fall
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
