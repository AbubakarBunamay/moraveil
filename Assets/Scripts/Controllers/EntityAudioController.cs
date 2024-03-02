using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] enemySounds; // Array of enemy sounds
    [SerializeField] private float soundTriggerDistance = 5.0f; // Distance to trigger enemy sounds
    
    private AudioSource audioSource; // Reference to AudioSource component
    private Transform player; // Reference to the player's transform

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Get the AudioSource component attached to the enemy
        audioSource = GetComponent<AudioSource>();

        // Start playing sounds
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            // Check if the player is within the trigger distance
            if (Vector3.Distance(transform.position, player.position) < soundTriggerDistance)
            {
                // Play a random sound from the array
                AudioClip randomSound = enemySounds[Random.Range(0, enemySounds.Length)];
                audioSource.PlayOneShot(randomSound);
            }

            // Wait for some time before checking again
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }
}
