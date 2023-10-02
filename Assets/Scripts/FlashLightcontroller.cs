using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightcontroller : MonoBehaviour
{
    public Light flashLight;
    private bool isFlashlightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
        flashLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(1) ) {
            ToggleFlashlight();
        }

    }

    private void ToggleFlashlight ()
    {
        isFlashlightOn = !isFlashlightOn;
        flashLight.enabled = isFlashlightOn;
        Debug.Log("Flashlight is on!");
    }
}
