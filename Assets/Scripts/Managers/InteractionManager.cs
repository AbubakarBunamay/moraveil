    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InteractionManager : MonoBehaviour{

    // Maximum distance for interaction
    public float maxInteractionDistance = 1f;

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Get all the colliders hit by the ray
            RaycastHit[] interactHits = Physics.RaycastAll(ray);

            // Iterate through all the hits and handle interactions
            foreach (RaycastHit hit in interactHits)
            {
                // Check if the hit is within the maximum interaction distance
                if (hit.distance <= maxInteractionDistance)
                {
                    HandleInteractable(hit.collider);
                    // Debug.Log("Hit: " + hit.collider.gameObject.name);
                }
            }
        }
    }

    // Handle interactions based on collider tags
    private void HandleInteractable(Collider collider)
    {
        // Check if the collider has the "Symbol" tag
        if (collider.CompareTag("Symbol"))
        {
            // Get the SymbolInteract component from the collider
            SymbolInteract symbol = collider.GetComponent<SymbolInteract>();

            // If the SymbolInteract component is not null, call its Interact method
            if (symbol != null)
            {
                symbol.Interact();
            }
        }
        // Check if the collider has the "KeypadButton" tag
        else if (collider.CompareTag("KeypadButton"))
        {
            // Get the KeypadButton component from the collider
            KeypadButton keypadButton = collider.GetComponent<KeypadButton>();

            // If the KeypadButton component is not null, call its KeypadClicked method
            if (keypadButton != null)
            {
                keypadButton.KeypadClicked();
            }
        }
        // Check if the collider has the "Map" tag
        else if (collider.CompareTag("Map"))
        {
            // Get the MapInteraction component from the collider
            MapInteraction mapInteraction = collider.GetComponent<MapInteraction>();

            // If the MapInteraction component is not null, call its InteractWithMap method
            if (mapInteraction != null)
            {
                mapInteraction.InteractWithMap();
            }
        }
        // Check if the collider has the "Mural" tag
        else if (collider.CompareTag("Mural"))
        {
            
        }
        // Check if the collider has the "Polaroid" tag
        else if (collider.CompareTag("Polaroid"))
        {
            // Get the PolaroidInteraction component from the collider
            Polaroid polaroid = collider.GetComponent<Polaroid>();

            if (polaroid != null)
            {
                // If the PolaroidInteraction component is not null, call its PolaroidInteract method
                polaroid.PolaroidInteract();
            }
        }
        // Check if the collider has the "Journal" tag
        else if (collider.CompareTag("Journal"))
        {
            
        }
        // If none of the specific tags match, get the generic Interactable component
        else
        {
            // Get the Interactable component from the collider
            Interactable interactable = collider.GetComponent<Interactable>();

            // If the Interactable component is not null, call its Interact method
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
