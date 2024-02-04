using System.Collections;
using UnityEngine;

public class Keypad : Interactable
{
    public KeypadManager manager; // Reference to the KeypadManager script
    public string correctCode = ""; // String to store the correct code
    private string enteredCode = ""; // String to store the entered code
    public GameObject Door; // Reference to the game object to move
    public float doorOpenSpeed = 2.0f; // Speed of the door opening animation
    public AudioSource doorAudioSource; // Reference to the AudioSource component on the door
    public AudioClip doorOpenSound; // Reference to the door opening sound

    // Method called when interacting with the keypad
    public override void Interact()
    {
        CheckInput(); // Call the CheckInput method
    }

    // Method to check the input code
    private void CheckInput()
    {
        manager = FindAnyObjectByType<KeypadManager>(); // Find the KeypadManager in the scene

        if (manager != null) // Check if the KeypadManager is found
        {
            if (correctCode.StartsWith(enteredCode)) // Check if the correct code starts with the entered code
            {
                if (enteredCode == correctCode) // Check if the entered code matches the correct code
                {
                    // Output a debug message indicating the correct password
                    Debug.Log("Password correct!");

                    // Start the coroutine for gradually opening the door
                    if (Door != null)
                    {
                        StartCoroutine(OpenDoor());
                    }
                }
                else
                {
                    // Continue checking for the next digit
                    Debug.Log("Correct input so far. Continue entering the password.");
                }
            }
            else
            {
                // Incorrect input, reset entered code and buttons in the KeypadManager
                enteredCode = "";
                // Reset the buttons in the KeypadManager only when the first digit is incorrect
                if (enteredCode.Length == 0)
                {
                    manager.ResetButtons();
                }
                Debug.Log("Incorrect input. Retry.");
            }
        }
        else
        {
            Debug.Log("KeypadManager not found"); // Output a debug message if KeypadManager is not found
        }
    }

    // Method to append a digit to the entered code
    public void AppendDigit(string digit)
    {
        // Check if the entered code is shorter than the correct code
        if (enteredCode.Length < correctCode.Length)
        {
            enteredCode += digit; // Append the digit to the entered code
            CheckInput(); // Check the input again
        }
        else
        {
            // Incorrect input, reset entered code
            enteredCode = "";
            Debug.Log("Incorrect input. Retry.");
            // Reset the buttons in the KeypadManager
            manager.ResetButtons();
        }
    }

    // Coroutine for gradually opening the door
    private IEnumerator OpenDoor()
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = Door.transform.position;
        Vector3 targetPosition = initialPosition + Vector3.up * 10f;

        // Play the door opening sound
        if (doorAudioSource != null && doorOpenSound != null)
        {
            doorAudioSource.PlayOneShot(doorOpenSound);
        }

        while (elapsedTime < 1f)
        {
            Door.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * doorOpenSpeed;
            yield return null;
        }

        // Ensure the door reaches the final position
        Door.transform.position = targetPosition;
    }
}
