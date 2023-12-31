using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoraveilSceneManager : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI; // Assign the pause menu UI in the Inspector.
    public GameObject restartMenuUI; // Assign the restart menu UI in the Inspector.
    public GameObject settingsMenuUI; // Assign the settings menu UI in the Inspector.
    public GameObject startMenuUI; // Assign the start menu UI in the Inspector.
    public GameObject HUD; // Assign the HUD in the Inspector.
    private bool isPlayerDead = false; // Variable to track player's life status.

    //Volume Sliders
    public Slider masterVolumeSlider; // Master Volume Slider
    public Slider musicVolumeSlider; // Music Volume Slider
    public Slider sfxVolumeSlider; // SFX Volume Slider
    
    private void Start()
    {
        // Make sure the menu is initially hidden.
        pauseMenuUI.SetActive(false);
        restartMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        
        //Game Starts with the Start menu which then launches player into the game
        StartMenu();
        
    }
    private void StartMenu()
    {
        // Pause the game
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause all audio 
        AudioListener.pause = true;

        // Hide the HUD when the restart prompt is shown 
        HUD.SetActive(false);

        // Unlock and show the cursor when the restart prompt is shown.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        startMenuUI.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {   
                if(!settingsMenuUI.activeSelf)
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
    
    public void SettingsUI()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause all audio
        AudioListener.pause = true;

        // Hide the HUD when the game is paused
        HUD.SetActive(false);

        // Unlock and show the cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Add listeners for volume sliders
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void BackButtonSetting()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true );
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

        // Pause the game.
        Time.timeScale = 0f;
        isGamePaused = true;

        // Pause all audio.
        AudioListener.pause = true;

        // Hide the HUD when the restart prompt is shown.
        HUD.SetActive(false);

        // Unlock and show the cursor when the restart prompt is shown.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideRestartPrompt()
    {
        // Hide the restart prompt UI.
        restartMenuUI.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
        isGamePaused = false;

        // Resume all audio
        AudioListener.pause = false;

        // Show the HUD when the restart prompt is hidden 
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
    
    //Volume Sliders
    public void SetMasterVolume(float volume)
    {
        SoundManager.instance.SetMasterVolume(volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        SoundManager.instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    
    // Start Button Action 
    public void StartGameButton()
    { 
        startMenuUI.SetActive(false); // Hide the start menu
        ResumeGame(); // Start the game
    }
}