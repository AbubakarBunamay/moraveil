using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Serializable class to represent a pair of audio clip and its corresponding subtitle
[System.Serializable]
public class AudioSubtitlePair
{
    public AudioClip audioClip; // Audio clip to be played
    public string subtitle; // Subtitle associated with the audio clip
}

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;  // Reference to the TextMeshProUGUI for displaying subtitles
    public List<AudioSubtitlePair> audioSubtitlePairs = new List<AudioSubtitlePair>(); // List of audio-subtitle pairs

    private AudioSource audioSource; // Reference to the AudioSource component
    private int currentSubtitleIndex = 0; // Index to keep track of the current audio-subtitle pair

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        audioSource.playOnAwake = false; // Make sure the audio doesn't play on awake

        // Check if there are audio-subtitle pairs provided
        if (audioSubtitlePairs.Count > 0)
        {
            PlayNextAudio(); // Play the first audio clip
        }
        else
        {
            Debug.Log("No audio-subtitle pairs provided.");
        }
    }

    void Update()
    {
        // Check if the audio clip has finished playing
        if (!audioSource.isPlaying)
        {
            currentSubtitleIndex++;

            // Check if there are more audio-subtitle pairs to play
            if (currentSubtitleIndex < audioSubtitlePairs.Count)
            {
                // Play the next audio clip and update the subtitle
                PlayNextAudio();
                UpdateSubtitleText();
            }
            else
            {
                // All audio tracks have been played
                Debug.Log("All audio tracks played.");
            }
        }
    }

    // Method to play the next audio clip in the list
    void PlayNextAudio()
    {
        // Play the audio clip from the current audio-subtitle pair
        audioSource.clip = audioSubtitlePairs[currentSubtitleIndex].audioClip;
        audioSource.Play();
    }
    
    // Method to trigger the subtitle manually (e.g., from another script or UI button)
    public void TriggerSubtitle()
    {
        // Check if there are more audio-subtitle pairs to play
        if (currentSubtitleIndex < audioSubtitlePairs.Count)
        {
            // Play the next audio clip and update the subtitle
            PlayNextAudio();
            UpdateSubtitleText();
        }
        else
        {
            Debug.Log("All audio tracks played.");
        }
    }
    
    // Method to update the subtitle text with the corresponding subtitle for the current audio track
    void UpdateSubtitleText()
    {
        // Update the subtitle text with the corresponding subtitle for the current audio track
        subtitleText.text = audioSubtitlePairs[currentSubtitleIndex].subtitle;
    }
}
