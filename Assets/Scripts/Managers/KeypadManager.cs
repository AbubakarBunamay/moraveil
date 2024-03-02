using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    [SerializeField] private Transform[] buttons; // Array to hold the transforms of keypad buttons
    
    private bool isPushed = false; // Track whether a button is currently pushed
    private Vector3[] initialPositions; // Array to store the initial positions of the buttons
    private List<Transform> pushedButtons = new List<Transform>(); // List to store pushed buttons
    
    private void Start()
    {
        // Cache the initial positions of the buttons
        StoreInitialPositions();
    }

    // When interacting with a button
    public void Interact(Transform button)
    {
        // Call methods to push, move the button and play the button press sound
        PushCube(button);
        MoveButton(button);
        PlayButtonSound(button);
    }

    // Method to push a button and update the entered code
    private void PushCube(Transform button)
    {
        // Check if the button has not been pushed before
        if (!pushedButtons.Contains(button))
        {
            pushedButtons.Add(button); // Add the button to the list of pushed buttons
            //Debug.Log("Added"+ button.name);
        }

        // Set the isPushed flag to true
        isPushed = true;
    }

    // Method to see if pushed
    public bool IsPushed()
    {
        return isPushed;
    }

    // Method to move a button
    private void MoveButton(Transform button)
    {
        // Move only the clicked button to the pushed position
        button.Translate(Vector3.back * 0.2f);
    }

    public void ResetButtons()
    {
        // Reset all pushed buttons to their initial positions
        foreach (Transform pushedButton in pushedButtons)
        {
            int index = System.Array.IndexOf(buttons, pushedButton);
            if (index != -1) 

            {
                pushedButton.position = initialPositions[index]; // If the button is found, reset its position to the initial position
                //Debug.Log("Reset Keypad Buttons");

            }
        }

        // Clear the list of pushed buttons
        pushedButtons.Clear();

        // Reset flags
        isPushed = false;
    }

    // Method to store the initial positions of the buttons
    private void StoreInitialPositions()
    {
        // Store the initial positions of the buttons
        initialPositions = new Vector3[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            initialPositions[i] = buttons[i].position; // Store the initial position as a Vector3 in each button's script

        }
    }

    // Method to play the button press sound
    private void PlayButtonSound(Transform button)
    {
        // Check if the button has an AudioSource component attached
        AudioSource audioSource = button.GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            // Play the button press sound
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
