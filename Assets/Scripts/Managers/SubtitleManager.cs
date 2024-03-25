using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SubtitleManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI subtitleText; // Reference to the TextMeshProUGUI component for displaying subtitles

    private Coroutine subControl; // Coroutine for controlling the subtitle sequence
    private bool subtitleSequenceRunning; // Flag indicating whether a subtitle sequence is currently running
    private bool subtitlesEnabled = true; // Toggling the subtitles on and off

    
    // CueSubtitle method starts a new subtitle sequence
    public void CueSubtitle(SubtitleTexts subText)
    {
        // Check if subtitles are interruptible and a sequence is already running
        if(subtitleSequenceRunning)
        {
            return; // If so, do not start a new sequence
        }
        else
        {
            // If a sequence is running, stop it
            if (subtitleSequenceRunning)
            {
                StopCoroutine(subControl);
            }
        }
        
        // Start a new subtitle sequence coroutine
        subControl = StartCoroutine(SubtitleControl(subText));
    }

    // Coroutine for controlling the subtitle sequence
    IEnumerator SubtitleControl(SubtitleTexts subText)
    {
        subtitleSequenceRunning = true; // Set the flag indicating a sequence is running

        // Loop through each dialogue line in the SubtitleTexts data
        for (int i = 0; i < subText.dialogue.Length; i++)
        {
            // Set the subtitle text to the current dialogue line
            subtitleText.text = subText.dialogue[i];
            // Wait for the specified duration before proceeding
            yield return new WaitForSeconds(subText.pauseUntilNextLine[i]);
        }
        
        // Clear the subtitle text after the sequence finishes
        subtitleText.text = "";
        // Reset the flag indicating a sequence is running
        subtitleSequenceRunning = false;
    }
    
    public void ClearSubtitle()
    {
        // Clear the subtitle text
        subtitleText.text = "";
        // Reset the flag indicating a sequence is running
        subtitleSequenceRunning = false;
        StopCoroutine(subControl);
    }

    public void ToggleSubtitles()
    {
        subtitlesEnabled = !subtitlesEnabled; // Toggle the state of subtitlesEnabled

        subtitleText.enabled = subtitlesEnabled; // Update UI state based on the new value of subtitlesEnabled

    }
}
