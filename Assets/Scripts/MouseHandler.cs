using UnityEngine;

public class MouseHandler : MonoBehaviour
{
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
    }

    void Update()
    {
        // Capture the horizontal movement of the mouse (left/right) and adjust its speed.
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;

        // Capture the vertical movement of the mouse (up/down) and adjust its speed.
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        // Update the horizontal rotation based on mouse movement.
        yRotation += mouseX;

        // Update the vertical rotation based on mouse movement.
        xRotation -= mouseY;

        // Clamp the vertical rotation to restrict it between -90 and 90 degrees to prevent over-rotation.
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Apply the new rotations to the camera's Euler angles to control its orientation.
        cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
}
