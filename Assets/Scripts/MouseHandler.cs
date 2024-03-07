using Cinemachine;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cinemachine VirtualCamera component
    
    private float sensitivity = 3.0f; // Default sensitivity value
    
    public float Sensitivity
    {
        get { return sensitivity; }
        set
        {
            sensitivity = value;
            UpdateSensitivity(value);
        }
    }

    void Start()
    {
        if (virtualCamera != null)
        {
            UpdateSensitivityFromPlayerPrefs(); // Set initial sensitivity from UI sliders
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera not assigned on MouseHandler.");
        }
    }
    public void UpdateSensitivityFromPlayerPrefs()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        UpdateSensitivity(sensitivity);
    }
    
    // Method to update sensitivity values from UI sliders
    public void UpdateSensitivity( float value)
    {
        CinemachinePOV pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            pov.m_HorizontalAxis.m_MaxSpeed = value;
            pov.m_VerticalAxis.m_MaxSpeed = value;
        }
        PlayerPrefs.SetFloat("Sensitivity", value);
    }
}
