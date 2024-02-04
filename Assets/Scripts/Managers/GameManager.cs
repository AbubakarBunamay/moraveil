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


    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration; // Set the initial timer value
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Game is not over and map is picked up
        if (!isGameOver && isMapPickedUp)
        {
            // Decrement timer if game is not over and the map is picked up
            timer -= Time.deltaTime;

            // If the player has not reached the endpoint within a certain time, show game over
            if (timer <= 0f) 
            {
                ShowGameOver();
            }
        }
    }
    
    // Method to notify GameManager that the map has been picked up
    public void MapPickedUp()
    {
        isMapPickedUp = true;
        uiManager.StartTimer(timerDuration); // Call UIManager method to start the timer
    }

    // Method to show game over UI
    public void ShowGameOver()
    {
        isGameOver = true;
        uiManager.GameOverUI(); // Call UIManager method to show the GameOver UI
    }
}
