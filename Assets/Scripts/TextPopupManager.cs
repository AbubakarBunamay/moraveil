using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopupManager : MonoBehaviour
{
    public TextMeshProUGUI popupText; // Reference to the Text component
    public string defaultText = "Default message"; // Default message if no specific message is found
    public Dictionary<string, string> messageDictionary = new Dictionary<string, string>();

    private void Start()
    {
        // Initialize the message dictionary with trigger area names and associated messages
        messageDictionary["TextPopupTriggerArea1"] = "Message for Trigger Area 1";
        messageDictionary["TextPopupTriggerArea2"] = "Message for Trigger Area 2";
        // Add more trigger areas and messages as needed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger area
        {
            // Get the name of the trigger area
            string triggerAreaName = gameObject.name;
            string message;

            // Try to find a message associated with the trigger area name
            if (messageDictionary.TryGetValue(triggerAreaName, out message))
            {
                popupText.text = message;
            }
            else
            {
                // Use the default message if no specific message is found
                popupText.text = defaultText;
            }

            // Show the popup text
            popupText.gameObject.SetActive(true);
            Debug.Log("Text pop to appear");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player exited the trigger area
        {
            // Hide the popup text
            popupText.gameObject.SetActive(false);
        }
    }
}
