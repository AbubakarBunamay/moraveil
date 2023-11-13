using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public string correctCode = "1234";

    public Material pushedMaterial;
    public Material correctMaterial;
    public Material wrongMaterial;
    public Material originalMaterial;


    private bool isCorrect = false;
    private bool isPushed = false;

    private MeshRenderer cubeRender;

    private string enteredCode = "";

    private void Start()
    {
      
    }

    public void Interact(KeypadButton keyPad)
    {
        if(!isPushed)
        {
           // PushCube();
            
        }
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
        cubeRender.material = originalMaterial;
        Debug.Log("Keypad reset");
    }
}
