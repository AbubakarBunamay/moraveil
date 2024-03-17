using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogManager : MonoBehaviour
{
    private AudioSource voiceAudioSource;
    [SerializeField] private SubtitleManager subtitleManager; // Reference to the SubtitleManager
    
    [Header("AudioClips")]
    [SerializeField] private AudioClip[] stunVoiceLines;
    [SerializeField] private AudioClip lilypadVoiceLine; // Voice line for stepping on the lilypad
    [SerializeField] private AudioClip firstWaterVoiceLine; // Voice line for stepping on water
    [SerializeField] private AudioClip mapInteractionDialogue; // Voice line for stepping on water
    
    [Header("Subtitles")]
    [SerializeField] private SubtitleTexts lilypadSubtitle; // Subtitle for stepping on the lilypad
    [SerializeField] private SubtitleTexts firstWaterSubtitle; // Subtitle for stepping on water
    [SerializeField] private SubtitleTexts mapInteractionSubtitle; // Subtitle for map interaction
    [SerializeField] private SubtitleTexts[] stunSubtitles; // Array of subtitle texts for stun dialogues

    private StressManager stressManager; // Reference to the StressManager
    private bool isStunned = false;
    private bool hasSteppedOnLilypad = false; // Track if the player has stepped on the lilypad
    private bool hasBeenInWater = false; // Track if the player has been in water


    private void Start()
    {
        // Ensure the AudioSource component is assigned
        if (voiceAudioSource == null)
        {
            voiceAudioSource = GetComponent<AudioSource>();
        }
        
        // Find and assign the StressManager reference
        stressManager = FindObjectOfType<StressManager>();
        
    }

    private void Update()
    {
        // Check if the player is stunned in the StressManager
        if (stressManager.IsPlayerStunned())
        {
            if (!isStunned)
            {
                PlayStunDialogue();
                isStunned = true;
            }
        }
        else
        {
            isStunned = false;
        }
    }

    // Function to play a random stun dialogue
    private void PlayStunDialogue()
    {
        if (stunVoiceLines.Length > 0)
        {
            int randomIndex = Random.Range(0, stunVoiceLines.Length);
            voiceAudioSource.clip = stunVoiceLines[randomIndex];
            voiceAudioSource.Play();
            // Check if a corresponding subtitle exists
            if (randomIndex < stunSubtitles.Length && subtitleManager != null)
            {
                // Trigger the corresponding subtitle
                subtitleManager.CueSubtitle(stunSubtitles[randomIndex]);
            }
        }
    }
    
    // Function to play the lilypad dialogue
    public void PlayLilypadDialogue()
    {
        if (!hasSteppedOnLilypad && !voiceAudioSource.isPlaying && lilypadVoiceLine != null)
        {
            voiceAudioSource.clip = lilypadVoiceLine;
            voiceAudioSource.Play();
            hasSteppedOnLilypad = true;
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(lilypadSubtitle);
            }
        }
    } 
    
    // Function to play the first time in water dialogue
    public void PlayFirstTimeinWaterDialogue()
    {
        if (!hasBeenInWater && firstWaterVoiceLine != null)
        {
            voiceAudioSource.clip = firstWaterVoiceLine;
            voiceAudioSource.Play();
            hasBeenInWater = true;
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(firstWaterSubtitle);
            }
        }
    }
    
    // Function to play the map interaction Dialog
    public void PlayMapInteractionDialog()
    {
        if (mapInteractionDialogue != null)
        {
            voiceAudioSource.clip = mapInteractionDialogue;
            voiceAudioSource.Play();
            hasBeenInWater = true;
            // Trigger subtitle
            if (subtitleManager != null)
            {
                subtitleManager.CueSubtitle(mapInteractionSubtitle);
            }
        }
    }
}
