using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    // Camera Position
    public Transform CamTransform;

    // Update is called once per frame
    private void Update()
    {
        if (CamTransform != null)
        {
            // Get the Camera's rotation
            Quaternion playerRotation = CamTransform.rotation;

            // Calculate the Z-axis rotation based on Camera's Y-axis rotation
            float zRotation = -playerRotation.eulerAngles.y;

            // Rotate the compass image around the Z-axis to indicate the direction
            transform.rotation = Quaternion.Euler(0, 0, -zRotation);
        }
    }
}
