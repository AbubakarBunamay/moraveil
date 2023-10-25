using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    private Transform cameraTransform; // Reference to the camera's transform component.
    private Vector3 originalPosition; // Store the camera's original position for recovery.
    public float shakeDuration = 0f; // The duration of the camera shake.
    public float shakeMagnitude = 0.7f; // The magnitude or strength of the camera shake.
    public float recoverySpeed = 2.0f; // The speed at which the camera recovers from the shake.

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>(); // Initialize the cameraTransform.
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration; // Set the shake duration.
        shakeMagnitude = magnitude; // Set the shake magnitude.
        originalPosition = cameraTransform.localPosition; // Store the camera's original position.
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            // Calculate a random offset based on the shakeMagnitude.
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

            // Preventing the camera to move in the Z-axis.
            randomOffset.z = 0; 

            // Lerp the camera's position to simulating a smooth recovery from the shake.
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalPosition + randomOffset, Time.deltaTime * recoverySpeed);

            // Decrease shake duration over time.
            shakeDuration -= Time.deltaTime * recoverySpeed;
        }
        else
        {
            shakeDuration = 0f; // Reset the shake duration when it's finished.
            cameraTransform.localPosition = originalPosition; // Restore the camera to its original position.
        }
    }
}
