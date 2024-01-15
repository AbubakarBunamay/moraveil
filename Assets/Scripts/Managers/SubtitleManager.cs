using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class AudioSubtitlePair
{
    public AudioClip audioClip;
    public string subtitle;
}

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;
    public List<AudioSubtitlePair> audioSubtitlePairs = new List<AudioSubtitlePair>();

    private AudioSource audioSource;
    private int currentSubtitleIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (audioSubtitlePairs.Count > 0)
        {
            PlayNextAudio();
        }
        else
        {
            Debug.LogWarning("No audio-subtitle pairs provided.");
        }
    }

    void Update()
    {
        // Check if the audio clip has finished playing
        if (!audioSource.isPlaying)
        {
            currentSubtitleIndex++;
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

    void PlayNextAudio()
    {
        // Play the next audio clip
        audioSource.clip = audioSubtitlePairs[currentSubtitleIndex].audioClip;
        audioSource.Play();
    }
    
    public void TriggerSubtitle()
    {
        if (currentSubtitleIndex < audioSubtitlePairs.Count)
        {
            PlayNextAudio();
            UpdateSubtitleText();
        }
        else
        {
            Debug.Log("All audio tracks played.");
        }
    }

    void UpdateSubtitleText()
    {
        // Update the subtitle text with the corresponding subtitle for the current audio track
        subtitleText.text = audioSubtitlePairs[currentSubtitleIndex].subtitle;
    }
}
