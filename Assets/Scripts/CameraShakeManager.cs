using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;
    public float shakeDuration = 0f;
    public float shakeMagnitude = 0.7f;
    public float recoverySpeed = 2.0f;

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        originalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            // Calculate a random offset based on the shakeMagnitude.
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            randomOffset.z = 0; // Make sure the camera doesn't move in the Z-axis.

            //cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            // Add this inside your Update method.
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalPosition + randomOffset, Time.deltaTime * recoverySpeed);
            shakeDuration -= Time.deltaTime * recoverySpeed;

            // Decrease shake duration over time.
            shakeDuration -= Time.deltaTime * recoverySpeed;
        }
        else
        {
            shakeDuration = 0f;
            cameraTransform.localPosition = originalPosition;
        }
    }
}
