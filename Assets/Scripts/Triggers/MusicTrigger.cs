using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip; // The audio clip to play

    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        // Create an AudioSource for this GameObject
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
    }

    // Called when another Collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the GameObject entering the trigger has a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Play the music clip
            audioSource.Play();
        }
    }

    // Called when another Collider exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        // Check if the GameObject exiting the trigger has a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Stop the music clip
            audioSource.Stop();
        }
    }
}
