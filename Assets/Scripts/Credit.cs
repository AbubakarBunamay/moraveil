using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    [SerializeField] private Animator creditsAnimation;     // Animation component reference here

    // Start is called before the first frame update
    void Start()
    {
        // Unpause all audio
        AudioListener.pause = false;
        Time.timeScale = 1f; // Returns Time scale back 
    }

    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Skip the animation and load the next scene
            LoadNextScene();
        }
    }

    // Method to load the next scene
    public void LoadNextScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
}
