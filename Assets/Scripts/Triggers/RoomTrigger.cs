using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private  AudioClip soundToPlay;      // The audio clip to play when triggered.
    private AudioSource audioSource;   // Reference to the AudioSource component attached to the GameObject.
    private static AudioSource currentlyPlaying;  // Static reference to the currently playing audio source.
    [SerializeField] private  float fadeDuration = 1.0f;   // Duration of the crossfade.


    void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

        // Set the currentlyPlaying to the audioSource of this instance.
        currentlyPlaying = audioSource;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger area has a specific tag (in this case, "Player").
        if (other.CompareTag("Player"))
        {
            // Check if the audio is not already playing before starting it.
            if (currentlyPlaying != audioSource || !audioSource.isPlaying)
            {
                // Start crossfade.
                StartCoroutine(CrossfadeAudio());
            }
        }
    }

    IEnumerator CrossfadeAudio()
    {
        float timer = 0f;
        float initialVolume = currentlyPlaying.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            
            // Calculate the normalized volume based on the current time and fade duration.
            float normalizedVolume = Mathf.Lerp(initialVolume, 0f, timer / fadeDuration);

            // Update the volume of the currently playing audio source.
            currentlyPlaying.volume = normalizedVolume;

            yield return null;
        }

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

        // Reset the volume of the newly started audio source.
        currentlyPlaying.volume = 1.0f;
    }
}
