using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine virtual camera.
    
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise; // Reference to the Cinemachine noise settings.
    private Vector3 originalPosition; // Store the camera's original position for recovery.
    private float shakeTimer = 0f; // Timer to control the duration of the shake.

    private float defaultFrequencyGain; // Default frequency gain.
    private float defaultAmplitudeGain; // Default amplitude gain.
    private bool isStressShaking = false; // Flag to indicate if the camera shake was triggered by stress

    private void Start()
    {
        if (virtualCamera != null) // Initialize Virtual Camera
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            // Store default values
            defaultFrequencyGain = virtualCameraNoise.m_FrequencyGain;
            defaultAmplitudeGain = virtualCameraNoise.m_AmplitudeGain;
        }
        else
        {
            Debug.LogError("VirtualCamera Isn't found for shake");
        }
    }
    
    // Method to check if the camera shake was triggered by stress
    public bool IsStressShaking()
    {
        return isStressShaking;
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
        
        isStressShaking = true;

    }
    
    // Reset frequency and Amplitude for the camera shake to the default values method
    public void ResetCameraShake()
    {
        virtualCameraNoise.m_FrequencyGain = defaultFrequencyGain;
        virtualCameraNoise.m_AmplitudeGain = defaultAmplitudeGain;
        virtualCamera.transform.position = originalPosition; // Reset camera position.
        shakeTimer = 0f;
        
        // Reset isStressShaking flag
        isStressShaking = false;
    }
    
    // Method to trigger a camera shake with custom parameters
    public void TriggerCameraShake(float duration, float frequency, float amplitude)
    {
        if (shakeTimer <= 0f) // Check if the shake timer is not already running
        {
            if (virtualCameraNoise != null)
            {
                // Apply custom parameters to the noise profile
                virtualCameraNoise.m_FrequencyGain = frequency;
                virtualCameraNoise.m_AmplitudeGain = amplitude;

                // Store the camera's original position
                originalPosition = virtualCamera.transform.position;

                // Start the shake timer
                shakeTimer = duration;
                
            }
        }
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime; // Decrease shake timer over time.
            if (shakeTimer <= 0f)
            {
                // Reset frequency and amplitude when shake ends
                ResetCameraShake();
            }
        }
    }
}

