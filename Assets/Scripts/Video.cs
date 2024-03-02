using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    [SerializeField] private  VideoPlayer videoPlayer;

    private void OnEnable()
    {
        if (!videoPlayer)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += LoadNextScene;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= LoadNextScene;
    }

    private void LoadNextScene(VideoPlayer source)
    {
        SceneManager.LoadScene(1);
    }
}
