using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public Keypad keyPad;
    public string digit;

    public void KeypadClicked()
    {
        keyPad.AppendDigit(digit);
    }
}
