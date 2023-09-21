using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPScontroller : MonoBehaviour
{
    CharacterController characterController;
    public float MovementSpeed = 1;
    public float Gravity = 9.8f;
    private float velocity = 0;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Capture player input for movement in the horizontal (left/right) and vertical (forward/backward) direction.
        float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * MovementSpeed;

        // Move the character based on the input and adjust for frame rate (Time.deltaTime).
        characterController.Move((Vector3.right * horizontal + Vector3.forward * vertical) * Time.deltaTime);

        // Check if the character is currently grounded (on the ground).
        if (characterController.isGrounded)
        {
            // If grounded, reset the vertical velocity to zero (no vertical movement).
            velocity = 0;
        }
        else
        {
            // If not grounded, apply gravity to the vertical velocity.
            velocity -= Gravity * Time.deltaTime;

            // Move the character in the vertical direction using the calculated velocity.
            characterController.Move(new Vector3(0, velocity, 0));
        }
    }
}
