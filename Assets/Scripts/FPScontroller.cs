using System.Collections;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Character Height")]
    public Vector3 originalCenter; // Original Center of Character
    public float originalHeight; // Original Height of Character

    [Header("Movement")]
    public float movementSpeed = 5f; // Speed of character movement.
    public float runningSpeed = 5f; // Speed of running Movement
    public float gravity = 9.8f; // Gravity force applied to the character.

    [Header("Swimming")]
    public float swimSpeed = 5f; // Speed of character while swimming.
    public float swimRotationSpeed = 2f; // Rotation speed while swimming.
    public float waterGravity = 9.81f; // Gravity when underwater.
    public bool isUnderwater = false; // Flag indicating if the character is underwater.
    private Vector3 swimmingDirection; // Direction of swimming.
    public float camAmplitude = 0.1f; // Adjust as needed.
    public float camFrequency = 1.0f; // Adjust as needed.
    public float maxFloatOffset = 0.1f; // Maximum allowed float offset
    public float minFloatOffset = 1.0f; // Minimum allowed float offset
    private StressManager stressManager; // Reference to the StressManager script.
    private float timeUnderwater = 0.0f; // Time spent underwater
    public float swimmingStressDelay = 2.0f; // Delay Swim Stress

    [Header("Jumping")]
    public float jumpHeight = 1f; // Height of the character's jump.
    public int maxJumps = 2; // Maximum number of allowed jumps.

    private int jumpsPerformed = 0; // Number of jumps performed.
    private float verticalVelocity; // Vertical velocity of the character.
    private bool isJumping = false; // Flag indicating whether the character is currently jumping.
    private bool isCrouching = false; //Flag indicating whether the character is currently crouching
    private bool isRunning = false; // Flag Indcating whether the character is running

    private Transform cameraTransform;
    private Quaternion originalCameraRotation;

    private bool isCameraFloating = false;

    public GlowstickController glowstickController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get a reference to the CharacterController component.
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        stressManager = FindObjectOfType<StressManager>();

        cameraTransform = Camera.main.transform;

    }

    private void Update()
    {
        // Check if the game is paused
        if (SceneManager.isGamePaused)
        {
            // Freeze the camera's rotation when paused
            cameraTransform.localRotation = originalCameraRotation;
        }
        else
        {
            HandleMovement();
            HandleJump();

            //Cursor Locked
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Check if underwater 
            if (isUnderwater)
            {
                HandleSwimming();
            }
        }

        originalCameraRotation = cameraTransform.localRotation;


    }

    /*
    * 
    * Character Movement
    * 
    */

    private void HandleMovement()
    {

        // Get input for horizontal and vertical movement.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        HandleCrouch(); // Handling Crouch 
        HandleRunning(); //Handling Running


        // Get the camera's forward and right vectors.
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Project these vectors onto the XZ plane (ignore the vertical orientation).
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalizing the vectors to ensure consistent speed.
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on camera orientation.
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        // Calculate the movement speed based on whether the character is running or walking.
        float moveSpeed = isRunning ? runningSpeed : movementSpeed;


        // Apply movement speed to the movement direction.
        Vector3 move = moveDirection * moveSpeed;

        // Include vertical velocity if the character is in the air.
        if (!characterController.isGrounded)
        {
            move.y = verticalVelocity;
        }

        // Apply delta time to make movement frame rate independent.
        move *= Time.deltaTime;

        // Move the character using the calculated movement vector.
        characterController.Move(move);
    }

    /*
    * 
    * Running
    * 
    */

    private void HandleRunning()
    {
        //Get Input for running
        isRunning = !isCrouching && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (isRunning)
        {
            runningSpeed = 10f;
        }
        else
        {
            runningSpeed = 5f;
        }
    }

    /*
    * 
    * Crouching
    * 
    */

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {

                //Reduce the Characters Height
                characterController.center = originalCenter * 0.5f;
                characterController.height = originalHeight * 0.5f;
            }
            else
            {
                //Bring the Characters Height
                characterController.center = originalCenter;
                characterController.height = originalHeight;

            }
        }
    }

    /*
    * 
    * Jumping
    * 
    */
    
    private void HandleJump()
    {

        bool groundedPlayer = characterController.isGrounded;

        if (groundedPlayer)
        {
            ResetJump(); // Reset jump-related variables when grounded.
        }
      
            ApplyGravity(); // Apply gravity force when in the air.
            TryPerformDoubleJump(); // Attempt to perform a double jump.
        
    }

    // Resets jump-related variables when the character is grounded.
    private void ResetJump()
    {
        //verticalVelocity = 0f; // Reset vertical velocity when grounded.
        jumpsPerformed = 0; // Reset jump count when grounded.
        isJumping = false;
    }

    // Attempts to perform a regular jump if conditions are met.
    private void TryPerformJump()
    {
        if (!isJumping && Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calculate jump velocity. 
            isJumping = true; // Set jumping flag to true.
            jumpsPerformed++; // Increment the jump count.
        }
    }

    // Applies gravity force when the character is in the air.
    private void ApplyGravity()
    {
        if (!isUnderwater)
        {
            verticalVelocity -= gravity * Time.deltaTime; // Apply gravity force when not swimming.
        }
        else
        {
            verticalVelocity = 0f; // Cancel gravity when swimming.
        }
    }

    // Attempts to perform a double jump if conditions are met.
    private void TryPerformDoubleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calculate double jump velocity.
            isJumping = true; // Set jumping flag for double jump.
            jumpsPerformed++; // Increment jump count for double jump.
        }
    }

    /*
     * 
     * SWIMMING
     * 
     */
    private void HandleSwimming()
    {
        if (isUnderwater)
        {
            // Get input for horizontal and vertical movement.
            float swimHorizontal = Input.GetAxis("Horizontal");
            float swimVertical = Input.GetAxis("Vertical");

            // Swim Controls
            Vector3 swimDirection = Camera.main.transform.forward * swimVertical + Camera.main.transform.right * swimHorizontal;

            // Modify swimmingDirection to include vertical movement.
            swimmingDirection = swimDirection.normalized * swimSpeed;

            // Calculate the imaginary dot between the camera's forward direction and the world up vector.
            float camDot = Vector3.Dot(Camera.main.transform.forward, Vector3.up);

            // Swim Up when looking up 
            if (camDot > 0.5f || Input.GetKey(KeyCode.Space)) 
            {
                swimmingDirection.y = -waterGravity * Time.deltaTime;
                Debug.Log("Swim UP");

            }
            // Swim Down when looking down
            else if (camDot < -0.5f || Input.GetKey(KeyCode.LeftControl)) 
            {
                swimmingDirection.y = waterGravity * Time.deltaTime;
                Debug.Log("Swim DOWN");

            }
            else
            {
                swimmingDirection.y = 0f; // Neutralize vertical movement when not looking up or down.
            }


            Camera.main.transform.Rotate(Vector3.up * swimHorizontal * swimRotationSpeed);


            if (isCameraFloating)
            {
                CameraFloatEffect(); // Camera Float Effect
            }

            //Swimming Stress Trigger

            timeUnderwater += Time.deltaTime; // Increase the time spent underwater while the player is underwater.


            if (timeUnderwater >= swimmingStressDelay)
            {
                stressManager.TriggerStress(); // Trigger stress when underwater for n seconds.
            }

        }
        else
        {
            // If the player is not in the water, set the swimmingDirection to zero to stop movement.
            swimmingDirection = Vector3.zero;

            // Reset the time spent underwater when the player is not underwater.
            timeUnderwater = 0.0f;

        }

        characterController.Move(swimmingDirection * Time.deltaTime);
    }

    // Simulate floating on when on water
    private void CameraFloatEffect()
    {

        // Calculate the float offset based on time.
        float floatOffset = Mathf.Sin(Time.time * camFrequency) * camAmplitude;

        // Apply the float offset to the camera's position.
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.y += floatOffset;

        // Update the camera's position.
        Camera.main.transform.position = cameraPosition;
    }


    /*
     * 
     * Triggers
     * 
     */

    public void OnTriggerEnter(Collider other)
    {   
        //Swimming Trigger
        if (other.CompareTag("Water"))
        {
            isUnderwater = true;
            isCameraFloating = true;
            Debug.Log("Got into Water");
            glowstickController.SetInWater(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Swimming Trigger
        if (other.CompareTag("Water"))
        {
            isUnderwater = false;
            isCameraFloating = false;
            Debug.Log("Out of Water");
            glowstickController.SetInWater(false);
        }
    }
}
