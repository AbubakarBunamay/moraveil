using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public AudioClip soundToPlay;      // The audio clip to play when triggered.
    private AudioSource audioSource;   // Reference to the AudioSource component attached to the GameObject.
    private static AudioSource currentlyPlaying;  // Static reference to the currently playing audio source.


    void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

        // Set the currentlyPlaying to the audioSource of this instance.
        currentlyPlaying = audioSource;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the audio is not already playing before starting it.
            if (currentlyPlaying != audioSource || !audioSource.isPlaying)
            {
                // Stop the currently playing audio source (if any).
                if (currentlyPlaying != null && currentlyPlaying.isPlaying)
                {
                    currentlyPlaying.Stop();
                }

                // Set the AudioClip for the AudioSource and play it.
                audioSource.clip = soundToPlay;
                audioSource.Play();

                // Update the currently playing reference.
                currentlyPlaying = audioSource;
            }
        }
    }
}
