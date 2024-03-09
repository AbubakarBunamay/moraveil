using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogManager : MonoBehaviour
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private AudioClip[] stunVoiceLines;
    private StressManager stressManager; // Reference to the StressManager
    private bool isStunned = false;


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
        }
    }
}
