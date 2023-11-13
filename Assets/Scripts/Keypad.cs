using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public Material pushedMaterial;
    public Material correctMaterial;
    public Material wrongMaterial;
    public KeypadManager manager;

    private bool isPushed = false;
    private Renderer cubeRenderer;
    private Material originalMaterial;


    private string enteredCode = ""; 

    private void Start()
    {
        cubeRenderer = GetComponentInChildren <Renderer>();
        originalMaterial = cubeRenderer.material;
    }

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
        cubeRenderer.material = pushedMaterial;
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
