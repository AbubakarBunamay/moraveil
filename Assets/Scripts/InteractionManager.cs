using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit interactHit;

            // Raycast for general interactables
            if (Physics.Raycast(ray, out interactHit))
            {
                Interactable interactable = interactHit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact(); // Interact with general object 
                }
            }

            // Raycast for symbols
            RaycastHit symbolHit;
            if (RaycastForSymbol(ray, out symbolHit))
            {
                SymbolInteract symbol = symbolHit.collider.GetComponent<SymbolInteract>();
                if (symbol != null)
                {
                    symbol.Interact(); // Interact with symbol
                }
            }
        }
    }

    private bool RaycastForSymbol(Ray ray, out RaycastHit hit)
    {
        int symbolLayer = LayerMask.NameToLayer("Symbol");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << symbolLayer))
        {
            SymbolInteract symbol = hit.collider.GetComponent<SymbolInteract>();
            if (symbol != null)
            {
                return true;
            }
        }

        return false;
    }
}
