using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Character Height")]
    public Vector3 originalCenter; // Original Center of Character
    public float originalHeight; // Original Height of Character

    [Header("Movement")]
    public float movementSpeed = 5f; // Speed of character movement.
    public float runningSpeed = 10f; // Speed of running Movement

    public float gravity = 9.8f; // Gravity force applied to the character.
    public Image crouchIcon; // Reference to the UI Image for the crouch icon

    [Header("Walking on Water")]
    public float walkOnWaterSpeed = 2.5f; // Adjust the speed as needed
    public float waterRunningSpeed = 5f; // Adjust the speed as needed
    private bool isWalkingOnWater = false; // Flag indicating whether the character is walking on water.

    [Header("Audio")]
    public AudioSource waterSound;

    [Header("Jumping")]
    public float jumpHeight = 1f; // Height of the character's jump.
    public int maxJumps = 2; // Maximum number of allowed jumps.
    private float maxJumpHeight; // Maximum Jump Height
    private float fallHeightThreshold = 2.0f; // Maximum Fall Height
    private int fallsCount = 0; // Falls Count

    [Header("Stress")]
    public StressManager stressManager; // Reference to the StressManager script.
    public float swimmingStressDelay = 2.0f; // Delay Swim Stress
    private float runTimer = 0f; // Timer to track how long the player has been running.
    public float maxRunTime = 5f; // Maximum allowed running time before triggering stress.
    public float runningStressIncreaseRate = 50f; // Rate of stress rising when running
    public float swimmingStressIncreaseRate = 50f; // Rate of stress rising when swimming
    public float swimmingStressDecreaseRate = 50f; // Rate of stress rising when swimming
    public float maxFallinStressRange = 1f; // Play with range of falling stress

    private int jumpsPerformed = 0; // Number of jumps performed.
    private float verticalVelocity; // Vertical velocity of the character.
    private bool isJumping = false; // Flag indicating whether the character is currently jumping.
    private bool isCrouching = false; //Flag indicating whether the character is currently crouching
    public bool isRunning = false; // Flag Indcating whether the character is running

    private Transform cameraTransform;
    private Quaternion originalCameraRotation;

    public GlowstickController glowstickController;

    public FootstepSound footstepSound;
    private MouseHandler m_Handler;

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get a reference to the CharacterController component.
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        stressManager = FindObjectOfType<StressManager>();
        footstepSound = FindObjectOfType<FootstepSound>();
        m_Handler = FindObjectOfType<MouseHandler>();

        cameraTransform = Camera.main.transform;

        crouchIcon.enabled = false;
    }

    private void Update()
    {
        // Check if the game is paused
        if (MoraveilSceneManager.isGamePaused)
        {
            m_Handler.enabled= false;
        }
        else
        {
            m_Handler.enabled = true;

            HandleMovement();
            HandleJump();

            // Check if the player is falling and trigger stress.
            //CheckFalls();

            //Cursor Locked
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                footstepSound.PlayFootstepSound("DefaultFootstep");
            }
            else
            {
                footstepSound.StopFootstepSound();
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

        // Calculate the movement speed based on whether the character is running, walking, or walking on water.
        float moveSpeed = isRunning ? runningSpeed : (isWalkingOnWater ? walkOnWaterSpeed : movementSpeed);

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
            runTimer += Time.deltaTime;

            // Set running speed based on whether the player is on water or not
            runningSpeed = isWalkingOnWater ? waterRunningSpeed : runningSpeed;

            // If the player has been running for longer than the allowed time, trigger stress.
            if (runTimer >= maxRunTime)
            {
                stressManager.IncreaseStress(Time.deltaTime * runningStressIncreaseRate);
                Debug.Log("Running");

            }
        }
        else
        {
            // If the player stops running and stress is at 0, reset the run timer.
            if (stressManager.currentStress == 0f)
            {
                runTimer = 0f;
            }
        }
    }

    /*
    * 
    * Crouching
    * 
    */

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {

                //Reduce the Characters Height
                characterController.center = originalCenter * 0.5f;
                characterController.height = originalHeight * 0.5f;

                // Show the crouch icon when crouching
                if (crouchIcon != null)
                {
                    crouchIcon.enabled = true;
                }
            }
            else
            {
                //Bring the Characters Height
                characterController.center = originalCenter;
                characterController.height = originalHeight;

                // Show the crouch icon when crouching
                if (crouchIcon != null)
                {
                    crouchIcon.enabled = false;
                }

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
            maxJumpHeight = transform.position.y; // Reset maxJumpHeight when grounded.
        }

        ApplyGravity(); // Apply gravity force when in the air.
        Jump(); // Perform a jump or double jump if conditions are met.

        // Update the maxJumpHeight during the jump.
        if (transform.position.y > maxJumpHeight)
        {
            maxJumpHeight = transform.position.y;
        }

    }

    // Resets jump-related variables when the character is grounded.
    private void ResetJump()
    {
        //verticalVelocity = 0f; // Reset vertical velocity when grounded.
        jumpsPerformed = 0; // Reset jump count when grounded.
        isJumping = false;
    }


    // Applies gravity force when the character is in the air.
    private void ApplyGravity()
    {
            verticalVelocity -= gravity * Time.deltaTime; 
    }

    // Perform a jump or double jump if conditions are met..
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps && !isCrouching)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calculate double jump velocity.
            isJumping = true; // Set jumping flag for double jump.
            jumpsPerformed++; // Increment jump count for double jump.
        }
    }


    // This checks if the player is currently falling and triggers stress
    private void CheckFalls()
    {
        // Check if the player is falling (not grounded and moving downward).
        if (!characterController.isGrounded && characterController.velocity.y < 0)
        {
            // Calculate the fall height based on the highest point reached during the jump.
            float fallHeight = maxJumpHeight - transform.position.y;

            // Check if the fall height is greater than the specified threshold.
            if (fallHeight > fallHeightThreshold)
            {
                // Increment the falls count.
                fallsCount++;

                // If the player falls twice their height, trigger stress.
                if (fallsCount >= 2)
                {
                    // Calculate stress increase as a percentage of the fall height.
                    float stressIncreasePercentage = Mathf.Clamp(fallHeight / fallHeightThreshold, 0f, maxFallinStressRange); 
                    float stressIncrement = 20f; 

                    // Calculate the actual stress increase.
                    float stressIncrease = stressIncreasePercentage * stressIncrement;

                    // Clamp the stress increase to a maximum value.
                    //stressIncrease = Mathf.Clamp(stressIncrease, 0f, 20f); // Adjust the upper limit as needed.

                    // Increment stress by the calculated amount
                    stressManager.IncreaseStress(stressIncrease);

                    // Reset falls count.
                    fallsCount = 0;
                }
            }
            else
            {
                // Reset falls count when the fall height is pointless.
                fallsCount = 0;
            }
        }
        else
        {
            // Reset falls count when grounded.
            fallsCount = 0;

            //If the player not falling decrease stress
            stressManager.DecreaseStress(stressManager.stressDecreaseRate * Time.deltaTime);

        }
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        characterController.enabled = isEnabled;
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
            isWalkingOnWater = true; // Set the flag when entering water
            Debug.Log("Walking on Water");
            glowstickController.SetInWater(true);
            if (!waterSound.isPlaying)
            {
                waterSound.Play();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Swimming Trigger
        if (other.CompareTag("Water"))
        {
            isWalkingOnWater = false; // Set the flag when entering water
            Debug.Log("Out of Water");
            glowstickController.SetInWater(false);

            // Stop water sound if it's playing
            if (waterSound.isPlaying)
            {
                waterSound.Stop();
            }
        }
    }
}
