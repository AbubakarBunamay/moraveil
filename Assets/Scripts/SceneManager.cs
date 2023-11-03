using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI; // Assign the pause menu UI in the Inspector.
    public GameObject HUD; // Assign the HUD in the Inspector.

    private void Start()
    {
        // Make sure the pause menu is initially hidden.
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause all audio
        AudioListener.pause = true;

        // Hide the HUD when the game is paused
        HUD.SetActive(false);

        // Unlock and show the cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

        // Show the HUD when the game is resumed
        HUD.SetActive(true);

        // Resume all audio
        AudioListener.pause = false;

        // Lock and hide the cursor when resumed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void QuitGame()
    {
        Debug.Log("Game Exiting");
        Application.Quit(); // Quit the game.
    }
}
