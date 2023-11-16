using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public KeypadManager manager; // Reference to the KeypadManager script
    public string correctCode = "1234"; // String to store the correct code
    private string enteredCode = ""; // String to store the entered code
    public GameObject Door; // Reference to the game object to move


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

                    // Reset the buttons in the KeypadManager
                    manager.ResetButtons();

                    // Move the game object up (adjust the value based on your needs)
                    if (Door != null)
                    {
                        Door.transform.Translate(Vector3.up * 10f);
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
                manager.ResetButtons();
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
            }
        
    }

}
