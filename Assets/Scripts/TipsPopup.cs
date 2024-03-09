using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupText; // Reference to the TextMeshProUGUI component
    [SerializeField] private string interactMessage = "Click to interact"; // Message to display when the player can interact
    [SerializeField] private string uninteractMessage = "Click to uninteract"; // Message to display when the player has interacted

    private bool hasInteracted = false; // Flag to track if the player has interacted

    private void Start()
    {
        // Hide the popup text initially
        HidePopupText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player.
        if (other.CompareTag("Player"))
        {
            // Update the popup text to show the interact message.
            UpdatePopupText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player.
        if (other.CompareTag("Player"))
        {
            // Hide the popup text.
            HidePopupText();
        }
    }

    private void UpdatePopupText(bool isInteractable)
    {
        // Use the interact message if the player can interact, otherwise use the uninteract message.
        string text = isInteractable ? interactMessage : uninteractMessage;
        SetPopupText(text);
    }

    private void SetPopupText(string text)
    {
        // Set the text of the popup and make it visible.
        if (popupText != null)
        {
            popupText.text = text;
            popupText.gameObject.SetActive(true);
        }
    }

    private void HidePopupText()
    {
        // Hide the popup text.
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    // Method to handle player interaction
    public void OnPlayerInteract()
    {
        // Toggle the interaction state
        hasInteracted = !hasInteracted;

        // Update the popup text based on the new interaction state
        UpdatePopupText(!hasInteracted);
    }
}
