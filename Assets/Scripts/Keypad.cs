using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public Material pushedMaterial;
    public Material correctMaterial;
    public Material wrongMaterial;

    private bool isPushed = false;
    private bool isCorrect = false;
    private int wrongInputs = 0;
    private int maxWrongInputs = 5;

    private Renderer cubeRenderer;
    private Material originalMaterial;
    private string correctCode = "1234"; 

    private void Start()
    {
        cubeRenderer = GetComponent <Renderer>();
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
        if (correctCode == "1234") // Replace with your actual correct code
        {
            isCorrect = true;
            cubeRenderer.material = correctMaterial;
            Debug.Log("Correct input! Something happens...");
        }
        else
        {
            wrongInputs++;
            cubeRenderer.material = wrongMaterial;
            Debug.Log("Wrong input.");

            if (wrongInputs >= maxWrongInputs)
            {
                ResetKeypad();
                Debug.Log("Too many wrong inputs. Resetting...");
            }
        }
    }

    private void ResetKeypad()
    {
        isPushed = false;
        isCorrect = false;
        wrongInputs = 0;
        cubeRenderer.material = originalMaterial;
        Debug.Log("Keypad reset.");
    }
}
