using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera.
    
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise; // Reference to the Cinemachine noise settings.
    private Vector3 originalPosition; // Store the camera's original position for recovery.
    private float shakeTimer = 0f; // Timer to control the duration of the shake.

    private float defaultFrequencyGain = 0f; // Default frequency gain.
    private float defaultAmplitudeGain = 1f; // Default amplitude gain.

    private void Start()
    {
        if (virtualCamera != null) // Initialize Virtual Camera
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            Debug.LogError("VirtualCamera Isn't found for shake");
        }
    }
    
    // Stress Camera Shake Method
    public void StressShakeCamera(float stressLevel, float maxStress, float maxFrequency, float maxAmplitude)
    {
        if (virtualCameraNoise != null)
        {
            // Calculate normalized stress
            float normalizedStress = Mathf.Clamp01(stressLevel / maxStress);

            // Adjust frequency and amplitude based on stress
            float frequency = Mathf.Lerp(0, maxFrequency, normalizedStress);
            float amplitude = Mathf.Lerp(0, maxAmplitude, normalizedStress);

            // Apply changes to the noise profile
            virtualCameraNoise.m_FrequencyGain = frequency;
            virtualCameraNoise.m_AmplitudeGain = amplitude;
        }
        
        // Store the camera's original position only once
        if (originalPosition == Vector3.zero)
        {
            originalPosition = virtualCamera.transform.position;
        }
    }
    
    // Reset frequency and Amplitude for the camera shake to the default values method
    public void ResetCameraShake()
    {
        virtualCameraNoise.m_FrequencyGain = defaultFrequencyGain;
        virtualCameraNoise.m_AmplitudeGain = defaultAmplitudeGain;
        virtualCamera.transform.position = originalPosition; // Reset camera position.
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime; // Decrease shake timer over time.
            if (shakeTimer <= 0f)
            {
                virtualCamera.transform.position = originalPosition; // Reset camera position.
            }
        }
    }
}

