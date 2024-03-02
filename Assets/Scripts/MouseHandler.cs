using Cinemachine;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    
    [SerializeField] private UIManager uiManager; // Reference to the UI manager for accessing sensitivity sliders
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cinemachine VirtualCamera component
    
    void Start()
    {
        if (virtualCamera != null && uiManager != null)
        {
            UpdateSensitivityFromUI(); // Set initial sensitivity from UI sliders

            // Register listeners for sensitivity sliders
            uiManager.horizontalSensitivitySlider.onValueChanged.AddListener(UpdateHorizontalSensitivity);
            uiManager.verticalSensitivitySlider.onValueChanged.AddListener(UpdateVerticalSensitivity);
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera or UIManager not assigned on MouseHandler.");
        }
    }

    // Method to update sensitivity values from UI sliders
    public void UpdateSensitivityFromUI()
    {
            CinemachinePOV pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

            // Set horizontal and vertical sensitivity from UI sliders
            if (pov != null)
            {
                // Adjusting the axis sensitivity directly
                pov.m_HorizontalAxis.m_MaxSpeed = uiManager.horizontalSensitivitySlider.value;
                pov.m_VerticalAxis.m_MaxSpeed = uiManager.verticalSensitivitySlider.value;
            }
    }

    // Method to update horizontal sensitivity
    public void UpdateHorizontalSensitivity(float value)
    {
        UpdateSensitivityFromUI();
    }

    // Method to update vertical sensitivity
    public void UpdateVerticalSensitivity(float value)
    {
        UpdateSensitivityFromUI();
    }
}
