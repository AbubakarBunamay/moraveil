using TMPro;
using UnityEngine;

public class TextPopupTrigger : MonoBehaviour
{
    [SerializeField] private  TextMeshProUGUI popupText; // Reference to the TextMeshProUGUI component
    [SerializeField] private  string message = "It Works!"; // The message to be displayed in the popup.
    
    private string defaultText = "Default message"; // Default message if no specific message is found

    private void Start()
    {
        // Hide the popup text initially
        HidePopupText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player.
        if (IsPlayer(other))
        {
            // Update the popup text.
            UpdatePopupText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player.
        if (IsPlayer(other))
        {
            // Hide the popup text.
            HidePopupText();
        }
    }

    private void UpdatePopupText()
    {
        // Use the default message if no specific message is found.
        SetPopupText(message != null ? message : defaultText);
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

    private bool IsPlayer(Collider other)
    {
        // Check if the collider's object has the "Player" tag.
        return other.CompareTag("Player");
    }
}
