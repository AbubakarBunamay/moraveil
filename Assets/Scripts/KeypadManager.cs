using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public string correctCode = "1234";


    private bool isCorrect = false;
    private bool isPushed = false;


    private string enteredCode = "";

    private void Start()
    {

    }

    public void Interact()
    {
        if(!isPushed)
        {
            PushCube();
            
        }
    }

    private void PushCube()
    {
        isPushed = true;
    } 

    public void AppendDigit ( string digit)
    {
        if (!isCorrect)
        {
            enteredCode += digit;
            //CheckCode();
        }
    }

    public void CheckCode()
    {
        if ( enteredCode.Length > correctCode.Length)
        {
            ResetKeypad();
        }
    }
    

    private void ResetKeypad ()
    {
        isPushed = false;
        isCorrect = false;
        enteredCode = "";
        Debug.Log("Keypad reset");
    }
}
