using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioClip defaultFootstepSound;
    public AudioClip grassFootstepSound;
    public AudioClip waterFootstepSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayFootstepSound(string soundType)
    {
        if (!audioSource.isPlaying)  // Check if the audio source is not currently playing
        {
            AudioClip footstepSound = GetFootstepSound(soundType);
            if (footstepSound != null)
            {
                audioSource.clip = footstepSound;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Footstep sound is null!");
            }
        }
    }

    private AudioClip GetFootstepSound(string soundType)
    {
        switch (soundType)
        {
            case "DefaultFootstep":
                return defaultFootstepSound;
            case "GrassFootstep":
                return grassFootstepSound;
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
