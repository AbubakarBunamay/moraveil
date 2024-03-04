using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipChanger : MonoBehaviour
{
    [SerializeField]
    private AudioSettings[] clips;

    [SerializeField]
    private AudioClip onTriggerEnterPlay;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.isPlaying == false)
        {
            int newClip = Random.Range(0, clips.Length);
            /*audioSource.volume = clips[newClip].vol;
            audioSource.pitch = clips[newClip].pitch;
            audioSource.clip = clips[newClip].clip;*/
            clips[newClip].SetupAudioSource(audioSource);
            audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //check other is the player.
        audioSource.clip = onTriggerEnterPlay;
        audioSource.Play();
    }
}

[System.Serializable]
public class AudioSettings
{
    public float vol;
    public float pitch;
    public AudioClip clip;

    public void SetupAudioSource(AudioSource audioSource)
    {
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.clip = clip;
        audioSource.Play();
    }
}