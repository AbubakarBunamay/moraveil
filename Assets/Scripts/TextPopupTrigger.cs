using TMPro;
using UnityEngine;

public class TextPopupTrigger : MonoBehaviour
{
    public TextMeshProUGUI popupText; // Reference to the TextMeshProUGUI component
    public string message = "It Works!";
    private string defaultText = "Default message"; // Default message if no specific message is found

    private void Start()
    {
        // Hide the popup text initially
        HidePopupText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            UpdatePopupText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            HidePopupText();
        }
    }

    private void UpdatePopupText()
    {
        // Customize this method to return a specific message or perform actions based on the attached object's logic.

        // Use the default message if no specific message is found
        SetPopupText(message != null ? message : defaultText);
    }

    private void SetPopupText(string text)
    {
        if (popupText != null)
        {
            popupText.text = text;
            popupText.gameObject.SetActive(true);
        }
    }

    private void HidePopupText()
    {
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }
}
