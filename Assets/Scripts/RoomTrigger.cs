using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public AudioClip soundToPlay;      // The audio clip to play when triggered.
    private AudioSource audioSource;   // Reference to the AudioSource component attached to the GameObject.
    public bool playOnce = true;       // Should the sound be played only once when triggered?

    private bool hasPlayed = false;    // Tracks whether the sound has been played.

    void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed || !playOnce)
        {
            // Check if the object entering the trigger area has a specific tag (in this case, "Player").
            if (other.CompareTag("Player"))
            {
                // Set the AudioClip for the AudioSource and play it.
                audioSource.clip = soundToPlay;
                audioSource.Play();

                // Mark that the sound has been played.
                hasPlayed = true;
            }
        }
    }
}
