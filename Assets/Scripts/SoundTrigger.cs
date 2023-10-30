using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    //public AudioClip clip2;
    public float volume = 3.7f;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();

        }

        audioSource.PlayOneShot(clip, volume);
    }

}