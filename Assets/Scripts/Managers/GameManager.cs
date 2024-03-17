using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public float timerDuration = 60f; // Initial timer duration in seconds
    public GameObject endpoint; // Reference to the GameObject with Collider representing the endpoint

    private bool isGameOver = false; // Game state if
    private bool isMapPickedUp = false; // Bool if map is picked up 
    private float timer; // Current timer value
    
    [SerializeField] private UIManager uiManager; // Reference to UIManager script
    [SerializeField] private FPSController player; // Reference to the player Object 
    
    // Triggers to be enabled after the map is picked up
    [SerializeField]  private Collider[] rockfallTriggersToEnable;
    
    // Triggers to be disabled after the map is picked up
    [SerializeField]  private Collider[] triggersToDisableOnMapPickup;
    
    [Header("Camera Shake")] // Camera Shake
    [SerializeField] private CameraShake cameraShake; // Camera Shake Reference
    // Minimum and maximum time intervals between camera shakes
    [SerializeField] private float minTimeBetweenShakes = 5f;
    [SerializeField] private float maxTimeBetweenShakes = 10f;
    [SerializeField] private float minShakeDuration = 1f;
    [SerializeField] private float maxShakeDuration = 5f;
    // Minimum and maximum values for frequency and amplitude
    [SerializeField] private float minFrequency = 1f;
    [SerializeField] private float maxFrequency = 4f;
    [SerializeField] private float minAmplitude = 1f;
    [SerializeField] private float maxAmplitude = 4f;
    private float timeUntilNextShake; // Time until the next camera shake

    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration; // Set the initial timer value
        player = FindObjectOfType<FPSController>(); // Find the player in the scene
        
        // Disable all triggers initially
        foreach (Collider trigger in rockfallTriggersToEnable)
        {
            trigger.enabled = false;
        }
        
        // Setting the initial time until the next shake
        timeUntilNextShake = Random.Range(minTimeBetweenShakes, maxTimeBetweenShakes);

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
                // Check if it's time to trigger a camera shake
                if (timeUntilNextShake <= 0f)
                {
                    // Trigger a camera shake
                    if (cameraShake != null)
                    {
                        // Randomize frequency,amplitude and shake duration within the specified ranges
                        float frequency = Random.Range(minFrequency, maxFrequency);
                        float amplitude = Random.Range(minAmplitude, maxAmplitude);
                        float shakeDuration = Random.Range(minShakeDuration, maxShakeDuration);

                        // Trigger the camera shake with random parameters
                        cameraShake.TriggerCameraShake(shakeDuration, frequency, amplitude);

                        // Reset the timer for the next shake
                        timeUntilNextShake = Random.Range(minTimeBetweenShakes, maxTimeBetweenShakes);
                    }
                }
                else
                {
                    // Decrease the time until the next shake
                    timeUntilNextShake -= Time.deltaTime;
                }
            }
        }
    }
    
    // Method to notify GameManager that the map has been picked up
    public void MapPickedUp()
    {
        isMapPickedUp = true;
        uiManager.StartTimer(); // Call UIManager method to start the timer
        uiManager.EnableTimerUI(); // Enable the timer UI when the map is picked up
        
        // Enable all the rockfall triggers after the map is picked up
        foreach (Collider trigger in rockfallTriggersToEnable)
        {
            trigger.enabled = true;
        }
        
        // Disable specified triggers after the map is picked up
        foreach (Collider trigger in triggersToDisableOnMapPickup)
        {
            trigger.enabled = false;
        }
        
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
    
    // Check if the player has reached the endpoint
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
