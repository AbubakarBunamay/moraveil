using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.VFX;

public class DestroyingEntity : MonoBehaviour
{
    public bool IsPlaying = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            IsPlaying = !IsPlaying;
        }

        if (IsPlaying)
        {
            GetComponent<VisualEffect>().Play();
        }
        else
        {
            GetComponent<VisualEffect>().Stop();
        }
    }
}