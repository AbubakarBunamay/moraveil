using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Static variables to track game state
    public static bool isGamePaused = false;
    public static bool isGameOnStart = false;
    
    // UI elements to be assigned in the Inspector
    public GameObject pauseMenuUI; 
    public GameObject restartMenuUI; 
    public GameObject settingsMenuUI; 
    public GameObject startMenuUI; 
    public GameObject exitMenuUI; 
    public GameObject HUD; 
    public GameObject creditsUI; 
    public GameObject FullScreencreditsUI; 
    public GameObject gameOverUI; 
    public TextMeshProUGUI UItimer;
    
    // Time Tracker
    private float timer = 0f;
    private bool isTimerRunning = false;
    
    // To track player's life status
    private bool isPlayerDead = false; 

    // Volume Sliders
    public Slider masterVolumeSlider; 
    public Slider musicVolumeSlider; 
    public Slider sfxVolumeSlider; 
    public Slider dialogVolumeSlider; 
    
    //Sensitivity Sliders
    public Slider horizontalSensitivitySlider;
    public Slider verticalSensitivitySlider;
    
    //References
    public RespawnManager respawnManager;
    public MouseHandler mouseHandler;
    public GameManager gameManager; // Reference to the GameManager script

    
    private void Start()
    {
        // Make sure the menu is initially hidden.
        pauseMenuUI.SetActive(false);
        restartMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        
        // Game Starts with the Start menu which then launches player into the game
        StartMenu();
        
    }

    private void Update()
    {
        // Check for the Escape key to handle pausing or going back
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOnStart)
        {
            if (isGamePaused)
            {
                // If the game is paused, check if not in settings to resume, otherwise go back
                if (!settingsMenuUI.activeSelf)
                {
                    ResumeGame();
                }
                else
                {
                    BackButton();
                }
            }
            else
            {
                // If the game is not paused, pause it
                PauseGame();
            }
        }
    }
    
    // Method to set up the Start menu
    private void StartMenu()
    {
        //Set State
        isGameOnStart = true;
        
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
        
        // Show the Start menu
        startMenuUI.SetActive(true);
    }
    
    // Method to handle the Start button action
    public void StartGameButton()
    { 
        startMenuUI.SetActive(false); // Hide the start menu
        isGameOnStart = false;         // Transition from start phase to gameplay
        ResumeGame(); // Start the game
    }
    
    // Method to toggle between full screen and windowed mode
    public void ToggleFullScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    // Method to handle back button functionality for different menus
    private void BackButton()
    {
        if (settingsMenuUI.activeSelf)
        {
            // If in settings menu, go back to pause menu
            BackButtonSetting();
        }
        // else if (startMenuUI.activeSelf)
        // {
        //     QuitGame();
        // }
        // else
        // {
        //     ResumeGame();
        // }
    }

    // Method to pause the game and show the pause menu
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show Pause Menu
        Time.timeScale = 0f; // Stop Time
        isGamePaused = true; // Set State

        // Pause all audio
        AudioListener.pause = true;

        // Hide the HUD when the game is paused
        HUD.SetActive(false);

        // Unlock and show the cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // Method to show the settings menu
    public void SettingsUI()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        settingsMenuUI.SetActive(true); // Show Settings Menu
        Time.timeScale = 0f; // Stop Time
        isGamePaused = true; // Set State

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
        dialogVolumeSlider.onValueChanged.AddListener(setDialogVolume);
        
        // Add listeners for sensitivity sliders
        horizontalSensitivitySlider.onValueChanged.AddListener(UpdateHorizontalSensitivity);
        verticalSensitivitySlider.onValueChanged.AddListener(UpdateVerticalSensitivity);
    }
    
    // Method to go back from the settings menu to the pause menu
    public void BackButtonSetting()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true );
    }
    
    // Method to go back from the start menu to the pause menu
    public void BackButtonStart()
    {
        settingsMenuUI.SetActive(false);
        startMenuUI.SetActive(true );
    }

    // Method to resume the game
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide Pause Menu
        Time.timeScale = 1f; // Continue Time
        isGamePaused = false; // Set State

        // Show the HUD when the game is resumed
        HUD.SetActive(true);

        // Resume all audio
        AudioListener.pause = false;

        // Lock and hide the cursor when resumed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 
    
    // Method to quit the game
    public void QuitGame()
    {   
        if(!settingsMenuUI.activeSelf)
            settingsMenuUI.SetActive(false);
        exitMenuUI.SetActive(true);
    }   
    
    // Method to exit the game
    public void ExitGame()
    {
        Debug.Log("Game Exiting");
        Application.Quit(); // Quit the game.
    }

    // Method called when the player dies to show the restart prompt
    public void PlayerDied()
    {
        // Show the restart prompt when the player dies.
        if (!isPlayerDead)
        {
            RestartUI();
            isPlayerDead = true;
        }
    }

    // Method to show the restart prompt
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

    // Method to hide the restart prompt and resume the game
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
        
        // Reset variables and state
        isPlayerDead = false;

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Method to restart the game by reloading the scene
    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Time Related 
    // Method to start the timer with an initial value
    public void StartTimer(float initialTimerValue)
    {
        timer = initialTimerValue;
        isTimerRunning = true;
        StartCoroutine(UpdateTimerDisplay());
    }

    // Coroutine to update the timer display
    private IEnumerator UpdateTimerDisplay()
    {
        while (isTimerRunning && timer > 0f)
        {
            timer -= Time.deltaTime; // Decrement the timer
            UpdateTimerUI();
            yield return null;
        }

        // If the timer reaches zero, show the game over UI or other relevant logic
        if (timer <= 0f)
        {
            gameManager.ShowGameOver();
        }
    }
    
    // Method to update the timer TextMeshProUGUI
    private void UpdateTimerUI()
    {
        if (UItimer != null)
        {
            // Display the timer as minutes and seconds
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);
            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            UItimer.text = timerString;
        }
    }
    
    // Methods to set different volume levels
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

    public void setDialogVolume(float volume)
    {
        SoundManager.instance.SetDialogueVolume(volume);
        PlayerPrefs.SetFloat("DialogueVolume", volume);
    }
    
    //Sensitivity Sliders
    public void UpdateHorizontalSensitivity(float value)
    {
        mouseHandler.UpdateHorizontalSensitivity(value);
    }

    public void UpdateVerticalSensitivity(float value)
    {
        mouseHandler.UpdateVerticalSensitivity(value);
    }
    
}