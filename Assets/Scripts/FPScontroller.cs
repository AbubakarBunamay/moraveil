using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FPScontroller : MonoBehaviour
{
    // Reference to the CharacterController component.
    CharacterController characterController;

    // Movement speed of the character.
    public float movementSpeed = 1;

    // Gravity force applied to the character.
    public float gravity = 9.8f;

    // Height of the character's jump.
    public float jumpHeight = 1f;

    // Maximum number of jumps allowed (2 for double jumping).
    public int maxJumps = 2;

    // Number of jumps performed.
    private int jumpsPerformed = 0;

    // Vertical velocity of the character.
    private float verticalVelocity;

    // Flag indicating whether the character is currently jumping.
    private bool isJumping = false;

    // Called when the script starts.
    private void Start()
    {
        // Get a reference to the CharacterController component attached to this GameObject.
        characterController = GetComponent<CharacterController>();
    }

    // Called once per frame.
    void Update()
    {
        // Check if the character is currently grounded (on the ground).
        bool groundedPlayer = characterController.isGrounded;

        // If the character is on the ground...
        if (groundedPlayer)
        {
            // Reset the vertical velocity when grounded.
            verticalVelocity = 0f;

            // Reset the jump count when grounded.
            jumpsPerformed = 0;

            // Allow jumping if not already jumping and the jump button is pressed.
            if (!isJumping && Input.GetButtonDown("Jump"))
            {
                // Calculate the jump velocity using physics formula.
                verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);
                isJumping = true; // Set jumping flag to true
                jumpsPerformed++; // Increment the jump count.
            }
        }
        else // If the character is in the air...
        {
            // Apply gravity force to the character's vertical velocity.
            verticalVelocity -= gravity * Time.deltaTime;

            // Check for double jump input if not already at the maximum jump count.
            if (Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps)
            {
                // Calculate the jump velocity for the double jump.
                verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);
                isJumping = true; // Set jumping flag to true
                jumpsPerformed++; // Increment the jump count for double jump.
            }
        }

        // Capture player input for movement in the horizontal (left/right) and vertical (forward/backward) direction.
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed;
        float vertical = Input.GetAxis("Vertical") * movementSpeed;

        // Create a movement vector based on the input and vertical velocity, adjusted for frame rate (Time.deltaTime).
        Vector3 move = new Vector3(horizontal, verticalVelocity, vertical) * Time.deltaTime;

        // Move the character based on the input and vertical velocity.
        characterController.Move(move);

        // Check if the character has landed to reset the jump flag.
        if (groundedPlayer)
        {
            isJumping = false; // Reset jumping flag when grounded.
        }
    }

}

