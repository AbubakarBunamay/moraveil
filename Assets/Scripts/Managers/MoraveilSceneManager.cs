using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoraveilSceneManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI; // Assign the pause menu UI in the Inspector.
    public GameObject restartMenuUI; // Assign the restart menu UI in the Inspector.
    public GameObject HUD; // Assign the HUD in the Inspector.
    private bool isPlayerDead = false; // New variable to track player's life status.


    private void Start()
    {
        // Make sure the pause menu is initially hidden.
        pauseMenuUI.SetActive(false);
        restartMenuUI.SetActive(false);


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

    public void PlayerDied()
    {
        // Show the restart prompt when the player dies.
        if (!isPlayerDead)
        {
            RestartUI();
            isPlayerDead = true;
        }
    }

    public void RestartUI()
    {
        // Show the restart prompt UI.
        restartMenuUI.SetActive(true);

        // Pause the game (optional: you may choose to let the game continue running).
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause all audio (optional: you may choose to let the audio continue playing).
        AudioListener.pause = true;

        // Hide the HUD when the restart prompt is shown (optional).
        HUD.SetActive(false);

        // Unlock and show the cursor when the restart prompt is shown.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideRestartPrompt()
    {
        // Hide the restart prompt UI.
        restartMenuUI.SetActive(false);

        // Resume the game (optional: you may choose to let the game continue running).
        Time.timeScale = 1f;
        isGamePaused = false;

        // Resume all audio (optional: you may choose to let the audio continue playing).
        AudioListener.pause = false;

        // Show the HUD when the restart prompt is hidden (optional).
        HUD.SetActive(true);

        // Lock and hide the cursor when the restart prompt is hidden.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void RestartGame()
    {
        HideRestartPrompt(); // Hide the restart UI before restarting the game.

        // Reset variables and state
        isPlayerDead = false;

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}