using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPScontroller : MonoBehaviour
{
    /*CharacterController characterController;
    public float movementSpeed = 1;
    public float Gravity = 9.8f;
    public float jumpHeight = 1f;
    private float velocity = 0;
    //Jump Direction
    private Vector3 jumpDirection = Vector3.zero;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Capture player input for movement in the horizontal (left/right) and vertical (forward/backward) direction.
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed;
        float vertical = Input.GetAxis("Vertical") * movementSpeed;

        // Move the character based on the input and adjust for frame rate (Time.deltaTime).
        characterController.Move((Vector3.right * horizontal + Vector3.forward * vertical ) * Time.deltaTime);
        
        // Check if the character is currently grounded (on the ground).
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            jumpDirection.y = jumpHeight;
            // If grounded, reset the vertical velocity to zero (no vertical movement).
            velocity = 0;
        }
        else
        {
            // If not grounded, apply gravity to the vertical velocity.
            velocity -= Gravity * Time.deltaTime;

            // Move the character in the vertical direction using the calculated velocity.
            characterController.Move(new Vector3(0, velocity, 0));

            jumpDirection.y += Physics.gravity.y * Gravity;
        }
        //
        
    }*/

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 0.01f;
    private float gravityValue = -9.81f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = true;//controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity);
    }
}
