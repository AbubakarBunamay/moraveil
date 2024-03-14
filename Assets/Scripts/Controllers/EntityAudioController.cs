using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudioController : MonoBehaviour
{
    [Header("AudioClips")]
    [SerializeField] private AudioClip[] entitySounds; // Array of enemy sounds
    
    [Header("Subtitle")]
    [SerializeField] private SubtitleTexts[] entitySubtitles; // Array of subtitle texts for entity dialogues

    [SerializeField] private float soundTriggerDistance = 5.0f; // Distance to trigger enemy sounds
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager

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
                int randomIndex = Random.Range(0, entitySounds.Length);
                AudioClip randomSound = entitySounds[randomIndex];
                audioSource.PlayOneShot(randomSound);
                
                // Check if a corresponding subtitle exists
                if (randomIndex < entitySubtitles.Length && subtitleManager != null)
                {
                    // Trigger the corresponding subtitle
                    subtitleManager.CueSubtitle(entitySubtitles[randomIndex]);
                }
            }

            // Wait for some time before checking again
            yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        }
    }
}
