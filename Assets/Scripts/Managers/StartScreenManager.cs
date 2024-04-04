using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    //UI Elements
    [Header("UI References")]
    [SerializeField] private GameObject startMenuUI; 
    [SerializeField] private GameObject settingsMenuUI; 
    [SerializeField] private GameObject exitMenuUI; 
    
    // Volume Sliders
    [Header("Slider References")]
    [SerializeField] private Slider masterVolumeSlider; 
    [SerializeField] private Slider musicVolumeSlider; 
    [SerializeField] private Slider sfxVolumeSlider; 
    [SerializeField] private Slider dialogVolumeSlider; 
    //Sensitivity Sliders
    public Slider sensitivitySlider;
    
    // Cinemachine VirtualCamera component
    [SerializeField] private CinemachineVirtualCamera virtualCamera; 
    [SerializeField] private MouseHandler mouseHandler; // Add reference to MouseHandler

    // Start is called before the first frame update
    void Start()
    {

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        settingsMenuUI.SetActive(false);
        
        // Start Menu
        StartMenu();
        
        // Load sensitivity from MouseHandler and update the slider value
        if (mouseHandler != null)
        {
            float sensitivity = mouseHandler.Sensitivity;
            sensitivitySlider.value = sensitivity;
            UpdateSensitivity(sensitivity);
        }
        
        // Return If Audio Listener is off to on
        AudioListener.pause = false;
        
        // Load volume and sensitivity values and update sliders
        LoadVolumeSettings();
        LoadSensitivitySettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Start menu
    private void StartMenu()
    {
        // Show the Start menu
        startMenuUI.SetActive(true);
        
    }

    // Start Game Button
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    // Credits Button 
    public void Credits()
    {
        SceneManager.LoadScene("Credit");
    }
    
    // Method to go back from the settings menu to the pause menu
    public void BackButtonSetting()
    {
        settingsMenuUI.SetActive(false);
        startMenuUI.SetActive(true );
    }
    
    // Method to toggle between full screen and windowed mode
    public void ToggleFullScreen()
    {
        Screen.fullScreenMode = Screen.fullScreenMode == FullScreenMode.Windowed ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    
    private void BackButton()
    {
        if (settingsMenuUI.activeSelf)
        {
            // If in settings menu, go back to pause menu
            settingsMenuUI.SetActive(false);
            startMenuUI.SetActive(true );
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
    
    public void SettingsUI()
    {
        startMenuUI.SetActive(false); // Hide Pause Menu
        settingsMenuUI.SetActive(true); // Show Settings Menu
        Time.timeScale = 0f; // Stop Time

        // Pause all audio
        AudioListener.pause = true;

        // Unlock and show the cursor when paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // Add listeners for volume sliders
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        dialogVolumeSlider.onValueChanged.AddListener(setDialogVolume);
        
        // Add listeners for sensitivity slider
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
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
    
    // Method to exit the game
    public void ExitGame()
    {
        Debug.Log("Game Exiting");
        Application.Quit(); // Quit the game.
        
        if (Application.isEditor)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
    
    // Method to quit the game
    public void QuitPopUp()
    {   
        exitMenuUI.SetActive(true);
    } 
    
    // Method to quit the game
    public void BackQuit()
    {   
        exitMenuUI.SetActive(false);
        startMenuUI.SetActive(true);
    } 
    
    public void UpdateSensitivity( float value)
    {
        Debug.Log("Sens changing");
        mouseHandler.UpdateSensitivity(value); // Update sensitivity in MouseHandler
        PlayerPrefs.SetFloat("Sensitivity", value);
    }
    
    // Method to load volume settings and update sliders
    private void LoadVolumeSettings()
    {
        // Load volume settings from PlayerPrefs 
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float dialogVolume = PlayerPrefs.GetFloat("DialogueVolume", 1f);

        // Update volume sliders
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        dialogVolumeSlider.value = dialogVolume;

        // Apply volume settings
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        setDialogVolume(dialogVolume);
    }

    // Method to load sensitivity settings and update sliders
    private void LoadSensitivitySettings()
    {
        // Load sensitivity settings from PlayerPrefs 
        float Sensitivity = PlayerPrefs.GetFloat("HorizontalSensitivity", 3f);

        // Update sensitivity sliders
        sensitivitySlider.value = Sensitivity;

        // Apply sensitivity settings
        UpdateSensitivity(Sensitivity);
    }
    
}
