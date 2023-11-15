using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] interactHits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in interactHits)
            {
                HandleInteractable(hit.collider);
                //Debug.Log("Hit: " + hit.collider.gameObject.name);
            }


        }
    }

    private void HandleInteractable(Collider collider)
    {
        if (collider.CompareTag("Symbol"))
        {
            SymbolInteract symbol = collider.GetComponent<SymbolInteract>();
            if (symbol != null)
            {
                symbol.Interact();
            }
        }
        else if(collider.CompareTag("KeypadButton"))
        {
            KeypadButton keypadButton = collider.GetComponent<KeypadButton>();
            if (keypadButton != null)
            {
                keypadButton.KeypadClicked();
            }

        }
        else
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
