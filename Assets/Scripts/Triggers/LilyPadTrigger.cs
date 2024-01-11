using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPadTrigger : MonoBehaviour
{
    public LilySoundManager lilySoundManager; // Referencing LilySoundManager 
    private AudioSource audioSource; // Audio Source of player

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play a random sound from the array
            int randomIndex = Random.Range(0, lilySoundManager.lilySounds.Length);
            audioSource.PlayOneShot(lilySoundManager.lilySounds[randomIndex]);
        }
    }
}
