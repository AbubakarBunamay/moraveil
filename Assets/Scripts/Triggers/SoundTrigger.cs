using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private  AudioClip soundToPlay;      // The audio clip to play when triggered.
    [SerializeField] private  bool playOnce = true;       // Should the sound be played only once when triggered?
    [SerializeField] private SubtitleTexts soundTriggerSubtitle; // Reference to the specific subtitle text

    private bool hasPlayed = false;    // Tracks whether the sound has been played.
    private SubtitleManager subtitleManager; // Reference to the subtitleManager
    [SerializeField] private AudioSource audioSource;   // Reference to the AudioSource component attached to the GameObject.

    void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        //audioSource = GetComponent<AudioSource>();

        // Find the SubtitleManager in the scene
        subtitleManager = GameObject.FindObjectOfType<SubtitleManager>();

        if (subtitleManager == null)
        {
            Debug.LogError("SubtitleManager not found in the scene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed || !playOnce && !audioSource.isPlaying)
        {
            // Check if the object entering the trigger area has a specific tag (in this case, "Player").
            if (other.CompareTag("Player"))
            {
                // Set the AudioClip for the AudioSource and play it.
                audioSource.clip = soundToPlay;
                audioSource.Play();
                // Trigger subtitle
                if (subtitleManager != null && soundTriggerSubtitle != null)
                {
                    subtitleManager.CueSubtitle(soundTriggerSubtitle);
                }          
                
                // Mark that the sound has been played.
                hasPlayed = true;
            }
        }
    }
    
}
