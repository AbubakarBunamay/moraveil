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
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * recoverySpeed;
        }
        else
        {
            shakeDuration = 0f;
            cameraTransform.localPosition = originalPosition;
        }
    }
}
