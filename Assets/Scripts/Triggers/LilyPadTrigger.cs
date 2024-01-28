using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadTrigger : MonoBehaviour
{
    public LilySoundManager lilySoundManager; // Referencing LilySoundManager 
    private AudioSource audioSource; // Audio Source of player

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger area has a specific tag (in this case, "Player").
        if (other.CompareTag("Player"))
        {
            // Play a random sound from the array
            int randomIndex = Random.Range(0, lilySoundManager.lilySounds.Length);
            audioSource.PlayOneShot(lilySoundManager.lilySounds[randomIndex]);
        }
    }
}
