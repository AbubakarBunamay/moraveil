using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private PlayerDialogManager playerDialogManager; // Reference to the PlayerDialogManager
    [SerializeField] private GameManager gameManager; // Reference to the GameManager script
    
    [Header("Materials to change")]
    [SerializeField] private List<GameObject> gameObjectToChange; // List of GameObjects to change the material of
    [SerializeField] private Material materialToChange; // Material to apply to the specified GameObjects
    
    [Header("Camera Shake")]
    [SerializeField] private CameraShake cameraShake; // Reference to the camera Shake
    [SerializeField] private float rumbleShakeDuration = 5f; // Rumble Camera Shake Duration
    [SerializeField] private float rumbleShakeFrequency = 3f; // Rumble Camera Shake Frequency
    [SerializeField] private float rumbleShakeAmplitude = 3f; // Rumble Camera Shake Amplitude
    
    [Header("Whisp References")]
    [SerializeField] private GameObject mapWhisp; // Reference to the Map Whisp
    [SerializeField] private GameObject whispGuide; // Reference to the Whisp Guide
    
    private void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>(); // Find the Camere Shake
        playerDialogManager = FindObjectOfType<PlayerDialogManager>(); // Find the player
        
        whispGuide.SetActive(false); // Initially deactivate whisp guide
    }
    
    // Interact with the map function 
    public void InteractWithMap()
    {
        mapWhisp.SetActive(false); // deactivate the map whisp
        whispGuide.SetActive(true); // activate the whisp guide
        
        foreach (GameObject objToChange in gameObjectToChange)
        {
            ChangeMaterialOftheObject(objToChange); // Change the material of each specified material
        }
        
        Disappear(); // Make the map disappear
        
        // Notify GameManager that the map has been picked up
        gameManager.MapPickedUp();
        
        //Trigger Camera Shake
        cameraShake.TriggerCameraShake(rumbleShakeDuration,rumbleShakeFrequency,rumbleShakeAmplitude);
        
        // Play the Map Pickup Dialog
        playerDialogManager.PlayMapInteractionDialog();
    }
    
    // Change the material of a specified materials
    private void ChangeMaterialOftheObject(GameObject objToChange)
    {
        if (objToChange != null)
        {
            Renderer render = objToChange.GetComponent<Renderer>(); // Get the Renderer component of the GameObject

            if (render != null)
            {
                render.material = materialToChange; // Change the material of the GameObject
            }
            else
            {
                Debug.Log("No Renderer attached to " + objToChange.name); // Log a warning if no Renderer is found
            }
        }
        else
        {
            Debug.Log("No Object to be replaced"); // Log a warning if the specified GameObject is null
        }
    }

    // Map Disappear 
    private void Disappear()
    {
        gameObject.SetActive(false); // Disable the GameObject, making it disappear
    }
}
