using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeralAudioScript : MonoBehaviour
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

//Plays a specific clip on an Audio Source once - stack music and ambience (Music, lily pad sound, keypad, tomb door open/cls)

//public class PlayAudio : MonoBehaviour
//{
//    public AudioSource audioSource;
//    public AudioClip clip;
//    public float volume = 0.5f;
//    void Start()
//    {
//        audioSource.PlayOneShot(clip, volume);
//    }
//}

//Plays an Audio Source at a precise time
//AudioSource.PlayScheduled(double timeToPlay);

//cue up continuous sound (door interact sound)
//public class PlayAudio : MonoBehaviour
//{
//    public audiosource audiosource1;
//    public audiosource audiosource2;
//    void start()
//    {
//        audiosource1.playscheduled(audiosettings.dsptime);
//        double cliplength = audiosource1.clip.samples / audiosource1.clip.frequency;
//        audiosource2.playscheduled(audiosettings.dsptime + cliplength);
//    }

//(impact/ jumping/player lading with force)
//public class PlayOnCollision : MonoBehaviour
//{
//    public AudioSource audioSource;
//    public float maxForce = 5;
//    void OnCollisionEnter(Collision collision)
//    {
//        float force = collision.relativeVelocity.magnitude;
//        float volume = 1;
//        if (force <= maxForce)
//        {
//            volume = force / maxForce;
//        }
//        audioSource.PlayOneShot(audioSource.clip, volume);
//    }
//}