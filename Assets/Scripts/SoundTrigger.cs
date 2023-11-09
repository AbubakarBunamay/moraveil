using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip soundToPlay;      // The audio clip to play when triggered.
    private AudioSource audioSource;   // Reference to the AudioSource component attached to the GameObject.
    public bool playOnce = true;       // Should the sound be played only once when triggered?

    private bool hasPlayed = false;    // Tracks whether the sound has been played.

    void Start()
    {
        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed || !playOnce)
        {
            // Check if the object entering the trigger area has a specific tag (in this case, "Player").
            if (other.CompareTag("Player"))
            {
                // Set the AudioClip for the AudioSource and play it.
                audioSource.clip = soundToPlay;
                audioSource.Play();

                // Mark that the sound has been played.
                hasPlayed = true;
            }
        }
    }
}

//public class SoundTrigger : MonoBehaviour
//{
//    public AudioSource audioSource;
//    public AudioClip clip;

//    //public AudioClip clip2;
//    public float volume = 3.7f;
//    void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Player" && !audioSource.isPlaying)
//        {
//            audioSource.clip = clip;
//            audioSource.Play();

//        }

//        audioSource.PlayOneShot(clip, volume);
//    }
//}

//run with velocity input 


// Audio for movementt 
//player walk
//public class Walking : MonoBehaviour
//{
//    public AudioSource sfx_human_walk1;

//    void //Update()
//    {
//        //if(Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D))
//        if(Input.GetKey(KeyCode.W | KeyCode.A | KeyCode.S | KeyCode.D))
//        {
//            sfx_human_walk1.enabled = true;
//        }
//            if (Input.GetKey(KeyCode.LeftShift))
//        {
//            sfx_human_walk1.enabled = true;
//        }
//        else
//        {
//            sfx_human_walk1.enabled = false;

//        }
//    }
//}

//player running 

//if (other.tag == "Player"
