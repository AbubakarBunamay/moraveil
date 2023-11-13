using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public KeypadManager keyPad;
    public string digit;


    public void KeypadClicked()
    {
        keyPad.AppendDigit(digit);
        Debug.Log("Input Digit:" + digit);
    }

}
