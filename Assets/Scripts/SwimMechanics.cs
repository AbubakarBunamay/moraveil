using UnityEngine;

public class SwimmingMechanics : MonoBehaviour
{
    private CharacterController characterController;
    private bool isUnderwater = false;

    [Header("Swimming")]
    public float movementSpeed = 5f;
    public float swimSpeed = 5f;
    public float swimRotationSpeed = 2f;
    public float buoyancy = 1f; // Default buoyancy force.
    public float waterGravity;


    [Header("Camera")]
    public Transform cameraTransform;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleSwimming();
    }

    private void HandleSwimming()
    {
        if (isUnderwater)
        {
            float swimHorizontal = Input.GetAxis("Horizontal");
            float swimVertical = Input.GetAxis("Vertical");

            Vector3 swimDirection = cameraTransform.forward * swimVertical + cameraTransform.right * swimHorizontal;

            // Apply movement speed to the swimming direction.
            Vector3 move = swimDirection * swimSpeed;

            // Include buoyancy force to simulate floating.
            move.y = buoyancy - waterGravity * Time.deltaTime;

            // Move the character using the calculated movement vector.
            characterController.Move(move * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = true;
            Debug.Log("Entered Water");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = false;
            Debug.Log("Got out Water");

        }
    }

}
