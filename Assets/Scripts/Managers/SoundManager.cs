using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Static instance to create a singleton pattern for the SoundManager.
    [SerializeField] private AudioMixer[] audioMixer; // Reference to the AudioMixer to control different audio groups.

    private void Awake()
    {
        // Singleton pattern: ensure only one instance of SoundManager exists.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // If another instance already exists, destroy this one.
        }
        DontDestroyOnLoad(gameObject); // Don't destroy the SoundManager when loading new scenes.
    }

    // Method to set the master volume level.
    public void SetMasterVolume( float volume)
    {
        foreach (AudioMixer audioMixer in audioMixer)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20); // Set the MasterVolume parameter in the AudioMixer based on the provided volume.
            }
        }
    }

    // Method to set the music volume level.
    public void SetMusicVolume( float volume)
    {
        foreach (AudioMixer audioMixer in audioMixer)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); // Set the MusicVolume parameter in the AudioMixer based on the provided volume.
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
                audioMixer.SetFloat("RoomTrigerVolume", Mathf.Log10(volume) * 20);
            }
        }
    }
    

    // Method to set the sound effects volume level.
    public void SetSFXVolume(float volume)
    {
        foreach (AudioMixer audioMixer in audioMixer)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20); // Set the SFXVolume parameter in the AudioMixer based on the provided volume.
                audioMixer.SetFloat("OneShotMixerVolume", Mathf.Log10(volume) * 20);
                audioMixer.SetFloat("WindVolume", Mathf.Log10(volume) * 20);
            }
        }
    }

    // Method to set the dialogue volume level.
    public void SetDialogueVolume(float volume)
    {
        foreach (AudioMixer audioMixer in audioMixer)
        {
            if (audioMixer != null)
            {
                audioMixer.SetFloat("DialogueVolume", Mathf.Log10(volume) * 20); // Set the DialogueVolume parameter in the AudioMixer based on the provided volume.
            }
        }
    }
}
