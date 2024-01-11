using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoFinish;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnVideoFinish(VideoPlayer vp)
    {
        //SceneManager.LoadScene("");
    }
}
