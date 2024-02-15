using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polaroid : MonoBehaviour
{
    private Animator animator;
    
    

    public void PolaroidInteract()
    {
        animator.SetTrigger("Polaroid_Animation");
    }
}
