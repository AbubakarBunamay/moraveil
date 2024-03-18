using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] private  AudioClip defaultFootstepSound; // Default footstep Sound
    [SerializeField] private  AudioClip grassFootstepSound; // Grass Footstep Sound
    [SerializeField] private  AudioClip waterFootstepSound; // Water Footstep sound

    private AudioSource audioSource; // Player Audio Source
    private string currentTerrainType; // Variable to store the current terrain type


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    // Play the foot step sounds
    public void PlayFootstepSound(string soundType)
    {
        // Get the footstep sound for the current terrain type
        AudioClip footstepSound = GetFootstepSound(soundType);

        // Check if the terrain type has changed or if the audio source is not currently playing
        if (currentTerrainType != soundType || !audioSource.isPlaying)
        {
            // Stop the currently playing footstep sound
            audioSource.Stop();

            // Set the new footstep sound
            if (footstepSound != null)
            {
                audioSource.clip = footstepSound;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Footstep sound is null!");
            }

            // Update the current terrain type
            currentTerrainType = soundType;
        }
    }
    
    // Play according to each terrain
    private AudioClip GetFootstepSound(string soundType)
    {
        switch (soundType)
        {
            case "DefaultFootstep":
                return defaultFootstepSound;
            case "WaterFootstep":
                return waterFootstepSound;
            default:
                return defaultFootstepSound;
        }
    }

    public void StopFootstepSound()
    {
       audioSource.Stop();
    }
}
