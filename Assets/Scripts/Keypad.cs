using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{

    public KeypadManager manager;

    private bool isPushed = false;


    private string enteredCode = ""; 

    public override void Interact()
    {
        if (!isPushed)
        {
            PushCube();
            CheckInput();
        }
    }

    private void PushCube()
    {
        isPushed = true;
    }

    private void CheckInput()
    {
        KeypadManager keypadManager = FindAnyObjectByType<KeypadManager>();

        if (keypadManager != null)
        {
            keypadManager.CheckCode();
        }
        else
        {
            Debug.Log("KeypadManager not found");
        }
        
    }

    public void AppendDigit(string digit)
    {
        if(!isPushed)
        {
            enteredCode = digit;
            CheckInput();
        }
    }

}
