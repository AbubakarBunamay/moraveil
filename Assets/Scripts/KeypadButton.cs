using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] private  KeypadManager keyPad; // Reference to the KeypadManager script
    [SerializeField] private  Keypad button; // Reference to the Keypad script
    [SerializeField] private  string digit; // String to store the digit associated with the button

    // Method called when the keypad button is clicked
    public void KeypadClicked()
    {
        // Call the Interact method of the KeypadManager, passing the button's transform
        keyPad.Interact(this.transform);

        // Call its AppendDigit method, passing the digit associated with the button & checks the input if correct 
        if (button != null)
        {
            button.AppendDigit(digit);
        }

        Debug.Log("Input Digit:" + digit);
    }

}
