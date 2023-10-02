using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        // Implement your custom interaction logic here
        Debug.Log("Interacted with " + gameObject.name);

    }
}
