using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    [SerializeField] private  KeypadManager manager; // Reference to the KeypadManager script
    [SerializeField] private  string correctCode = ""; // String to store the correct code
    [SerializeField] private  GameObject Door; // Reference to the game object to move
    [SerializeField] private  float doorOpenSpeed = 2.0f; // Speed of the door opening animation
    [SerializeField] private  AudioSource doorAudioSource; // Reference to the AudioSource component on the door
    [SerializeField] private  AudioClip doorOpenSound; // Reference to the door opening sound
    [SerializeField] private Material correctMaterial; // Material for correct button
    [SerializeField] private Material defaultMaterial; // Material for default button 
    [SerializeField] private Renderer[] buttonRenderers; // Renderers of the keypad buttons

    private string enteredCode = ""; // String to store the entered code
    
    // Method called when interacting with the keypad
    public override void Interact()
    {
        CheckInput(); // Call the CheckInput method
    }

    // Method to check the input code
    private void CheckInput()
    {
        if (manager != null) // Check if the KeypadManager is found
        {
            if (correctCode.StartsWith(enteredCode)) // Check if the correct code starts with the entered code
            {
                int buttonIndex = manager.GetButtonIndex(this.transform);

                if (enteredCode == correctCode) // Check if the entered code matches the correct code
                {
                    // Output a debug message indicating the correct password
                    Debug.Log("Password correct!");

                    // Start the coroutine for gradually opening the door
                    if (Door != null)
                    {
                        StartCoroutine(OpenDoor());
                    }
                    
                    // Change All Button Material to Correct 
                    //SetAllButtonMaterial(correctMaterial);
                    
                    // Disable button interaction
                    DisableButtonInteraction();
                }

                // Change the material of specific buttons to correct material
                //SetButtonMaterial(buttonIndex, correctMaterial);

            }
            else
            {

                // Reset the buttons in the KeypadManager only when the first digit is incorrect
                if (!correctCode.StartsWith(enteredCode))
                {
                    manager.ResetButtons();
                    // Incorrect input, reset entered code and buttons in the KeypadManager
                    enteredCode = "";
                    Debug.Log("Incorrect input. Retry.");
                    SetAllButtonMaterial(defaultMaterial);
                }
            }
        }
        else
        {
            Debug.Log("KeypadManager not found"); // Output a debug message if KeypadManager is not found
        }
    }
    
// Method to set material for specific button
    private void SetButtonMaterial(int buttonIndex, Material material)
    {
        if (buttonIndex >= 0 && buttonIndex < buttonRenderers.Length)
        {
            buttonRenderers[buttonIndex].material = material;
        }
    }
   
    private void SetAllButtonMaterial( Material material)
    {
        foreach (var renderer in buttonRenderers)
        {
            renderer.material = material;
        }
    }

    // Method to disable button interaction
    private void DisableButtonInteraction()
    {
        foreach (var button in manager.buttons)
        {
            button.GetComponent<Collider>().enabled = false;
            Debug.Log("Pass Correct so KeypadButtons Disabled");
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
