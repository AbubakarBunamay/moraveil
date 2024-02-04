using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timerDuration = 60f; // Initial timer duration in seconds

    private bool isGameOver = false; // Game state if
    private bool isMapPickedUp = false; // Bool if map is picked up 
    private float timer; // Current timer value
    
    public UIManager uiManager; // Reference to UIManager script

    public GameObject endpoint; // Reference to the GameObject with Collider representing the endpoint

    private FPSController player;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration; // Set the initial timer value
        // Find the player in the scene
        player = FindObjectOfType<FPSController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Game is not over and map is picked up
        if (!isGameOver && isMapPickedUp)
        {
            // If the player has reached the endpoint, show right UI
            if (CheckIfPlayerReachedEndPoint())
            {
                ShowGameOver();
            }
            else
            {
                // Decrement timer if the player is not in the endpoint trigger zone
                timer -= Time.deltaTime;

                // If the timer runs out, show game over
                if (timer <= 0f)
                {
                    ShowGameOver();
                }
            }
        }
    }
    
    // Method to notify GameManager that the map has been picked up
    public void MapPickedUp()
    {
        isMapPickedUp = true;
        uiManager.StartTimer(); // Call UIManager method to start the timer
        // Enable the timer UI when the map is picked up
        uiManager.EnableTimerUI();
    }

    // Method to show game over UI
    public void ShowGameOver()
    {
        if (player.IsInEndpointTriggerZone)
        {
            uiManager.ShowResultUI(true); // Show credits UI
        }
        else
        {
            uiManager.ShowResultUI(false); // Show game over UI
        }
    }
    
    private bool CheckIfPlayerReachedEndPoint()
    {
        // Check if the player has entered the trigger zone of the endpoint
        return player.IsInEndpointTriggerZone;
    }
    
    // Method to reset and start the timer
    public void ResetAndStartTimer()
    {
        timer = timerDuration; // Reset the timer to its initial value
        uiManager.ResetTimer(); // Reset and start the timer in UIManager
    }

}
