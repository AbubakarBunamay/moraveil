using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    // Reference to the UI manager for accessing sensitivity sliders
    public UIManager uiManager;
    
    // horizontal rotation speed
    public float horizontalSpeed = 2f;
    // vertical rotation speed
    public float verticalSpeed = 2f;
    //x rotation 
    private float xRotation = 0.0f;
    //y rotation 
    private float yRotation = 0.0f;
    //Camera
    private Camera cam;

    void Start()
    {
        cam = Camera.main; //Referencing Camera
        
        if (uiManager != null)
        {
            //Debug.Log("UIManager is not null");
            UpdateSensitivityFromUI(); // Set initial sensitivity from UI sliders
            
            // Register listeners for sensitivity sliders
            uiManager.horizontalSensitivitySlider.onValueChanged.AddListener(UpdateHorizontalSensitivity);
            uiManager.verticalSensitivitySlider.onValueChanged.AddListener(UpdateVerticalSensitivity);
        }
    }

    void Update()
    {
        // Capture the horizontal and vertical movement of the mouse and adjust their speeds.
        float mouseX = GetHorizontalInput() * horizontalSpeed;
        float mouseY = GetVerticalInput() * verticalSpeed;

        // Update the horizontal rotation based on mouse movement.
        yRotation += mouseX;

        // Update the vertical rotation based on mouse movement.
        xRotation -= mouseY;

        // Clamp the vertical rotation to restrict it between -90 and 90 degrees to prevent over-rotation.
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Apply the new rotations to the camera's Euler angles to control its orientation.
        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
    
    // Method to get horizontal mouse input
    private float GetHorizontalInput()
    {
        return Input.GetAxis("Mouse X");
    }

    // Method to get vertical mouse input
    private float GetVerticalInput()
    {
        return Input.GetAxis("Mouse Y");
    }
    // Method to update sensitivity values from UI sliders
    public void UpdateSensitivityFromUI()
    {
        // Set horizontal and vertical speeds from UI sliders
        horizontalSpeed = uiManager.horizontalSensitivitySlider.value;
        verticalSpeed = uiManager.verticalSensitivitySlider.value;
    }
    
    // Method to update horizontal sensitivity
    public void UpdateHorizontalSensitivity(float value)
    {
        horizontalSpeed = value;
    }

    // Method to update vertical sensitivity
    public void UpdateVerticalSensitivity(float value)
    {
        verticalSpeed = value;
    }
}
