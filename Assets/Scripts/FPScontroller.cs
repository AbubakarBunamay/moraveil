using System.Collections;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Movement")]
    public float movementSpeed = 5f; // Speed of character movement.
    public float runningSpeed = 5f; // Speed of running Movement

    [Header("Character Height")]
    public Vector3 originalCenter; // Original Center of Character
    public float originalHeight;   // Original Height of Character


    [Header("Jumping")]
    public float gravity = 9.8f; // Gravity force applied to the character.
    public float jumpHeight = 1f; // Height of the character's jump.
    public int maxJumps = 2; // Maximum number of allowed jumps.

    private int jumpsPerformed = 0; // Number of jumps performed.
    private float verticalVelocity; // Vertical velocity of the character.
    private bool isJumping = false; // Flag indicating whether the character is currently jumping.
    private bool isCrouching = false; //Flag indicating whether the character is currently crouching

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get a reference to the CharacterController component.
        originalHeight = characterController.height;
        originalCenter = characterController.center;
    }

    private void Update()
    {
        HandleMovement(); // Handle character movement.
        HandleJump(); // Handle jumping.
    }

    // Handles character movement based on player input.
    private void HandleMovement(){

        // Get input for horizontal and vertical movement.
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        //Crouch Mechanic
        if (Input.GetKeyDown(KeyCode.C))
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

        //Get Input for running
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);

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

    // Handles character jumping behavior.
    private void HandleJump(){

        bool groundedPlayer = characterController.isGrounded;

        if (groundedPlayer)
        {
            ResetJump(); // Reset jump-related variables when grounded.
            TryPerformJump(); // Attempt to perform a jump.
        }
        else
        {
            ApplyGravity(); // Apply gravity force when in the air.
            TryPerformDoubleJump(); // Attempt to perform a double jump.
        }
    }

    // Resets jump-related variables when the character is grounded.
    private void ResetJump(){
        verticalVelocity = 0f; // Reset vertical velocity when grounded.
        jumpsPerformed = 0; // Reset jump count when grounded.
    }

    // Attempts to perform a regular jump if conditions are met.
    private void TryPerformJump(){
        if (!isJumping && Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calculate jump velocity. 
            isJumping = true; // Set jumping flag to true.
            jumpsPerformed++; // Increment the jump count.
        }
    }

    // Applies gravity force when the character is in the air.
    private void ApplyGravity(){ 
    
        verticalVelocity -= gravity * Time.deltaTime; // Apply gravity force over time to simulate falling.
    }

    // Attempts to perform a double jump if conditions are met.
    private void TryPerformDoubleJump() {  
        if (Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calculate double jump velocity.
            isJumping = true; // Set jumping flag for double jump.
            jumpsPerformed++; // Increment jump count for double jump.
        }
    }

}
