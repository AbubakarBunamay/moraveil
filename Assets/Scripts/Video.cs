using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer videoPlayer; // VideoPlayer Reference Object
    
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.GetComponent<VideoPlayer>();         // Get the VideoPlayer component attached to this GameObject.
        videoPlayer.loopPointReached += OnVideoFinish;         // Subscribe to the loopPointReached event, which is triggered when the video finishes playing.

    }

    void OnVideoFinish(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainScene");         // Load the specified scene ("MainScene") when the video finishes.
    }
}
