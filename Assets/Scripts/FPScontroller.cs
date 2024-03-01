using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    private CharacterController characterController; // Character Controller

    [Header("Character Height")]
    public Vector3 originalCenter; // Original Center of Character
    public float originalHeight; // Original Height of Character

    [Header("Movement")]
    public float movementSpeed = 5f; // Speed of character movement.
    public float runningSpeed = 10f; // Speed of running Movement
    public float crouchRunningSpeed = 7f; // Speed when running while crouched
    public float gravity = 9.8f; // Gravity force applied to the character.
    public Image crouchIcon; // Reference to the UI Image for the crouch icon
    public GameObject headCube; // Reference to the cube object placed above the player's head
    private float uncrouchSpeed = 4f; // The speed of uncrouching
    private bool isUncrouching = false; // The player uncrouching state
    
    [HideInInspector]
    public float originalRunningSpeed; // Variable to store the original running speed

    [Header("Walking on Water")]
    public float walkOnWaterSpeed = 2.5f; // Adjust the speed as needed
    public float waterRunningSpeed = 5f; // Adjust the speed as needed
    private bool isWalkingOnWater = false; // Flag indicating whether the character is walking on water.

    [Header("Jumping")]
    public float jumpHeight = 1f; // Height of the character's jump.
    public int maxJumps = 2; // Maximum number of allowed jumps.
    private float maxJumpHeight; // Maximum Jump Height
    private float fallHeightThreshold = 2.0f; // Maximum Fall Height
    private int fallsCount = 0; // Falls Count

    [Header("Stress")]
    public StressManager stressManager; // Reference to the StressManager script.
    private float runTimer = 0f; // Timer to track how long the player has been running.
    public float maxRunTime = 5f; // Maximum allowed running time before triggering stress.
    public float runningStressIncreaseRate = 50f; // Rate of stress rising when running
    public float maxFallinStressRange = 1f; // Play with range of falling stress

    private int jumpsPerformed = 0; // Number of jumps performed.
    private float verticalVelocity; // Vertical velocity of the character.
    public bool isJumping = false; // Flag indicating whether the character is currently jumping.
    private bool isCrouching = false; //Flag indicating whether the character is currently crouching
    public bool isRunning = false; // Flag Indcating whether the character is running
    private bool canCrouch = true; // Add this line to store the crouch permission

    private Transform cameraTransform; // Reference to the main camera's transform.
    private Quaternion originalCameraRotation; // Stores the original rotation of the camera.

    public FootstepSound footstepSound; // Reference to the FootstepSound component.
    private MouseHandler mHandler; // Reference to the MouseHandler component.
    public GameManager gameManager; //Reference to the game manager
    private bool isInEndpointTriggerZone = false; // Bool if player passes end point
    
    private LilyPadTrigger currentLilypad; // Reference to the current lilypad the player is on


    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get a reference to the CharacterController component.
        
        // Store the original height and center of the character.
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        // Find and assign the components.
        stressManager = FindObjectOfType<StressManager>();
        footstepSound = FindObjectOfType<FootstepSound>();
        mHandler = FindObjectOfType<MouseHandler>();
        footstepSound = FindObjectOfType<FootstepSound>(); 

        // Get the main camera's transform and set the crouch icon to initially be disabled.
        cameraTransform = Camera.main.transform;
        crouchIcon.enabled = false;
        
        //Storing the original running speed
        originalRunningSpeed = runningSpeed; 
    }

    private void Update()
    {
        // Check if the game is paused
        if (UIManager.isGamePaused)
        {
            // Disable the MouseHandler when the game is paused.
            mHandler.enabled= false;
        }
        else
        {
            // Enable the MouseHandler when the game is not paused.
            mHandler.enabled = true;

            // Handle character movement and jumping.
            HandleMovement();
            HandleJump();

            // Check if the player is falling and trigger stress.
            //CheckFalls();

            // Lock the cursor to the center of the screen.
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        
        // Check if the player is on a lilypad
        if (currentLilypad != null)
        {
            // Synchronize player movement with lilypad
            currentLilypad.MovePlayerWithLilypad();
        }
        
        // Store the original rotation of the camera.
        originalCameraRotation = cameraTransform.localRotation;


    }
    
    private string CalculateTerrainType()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, characterController.radius * 0.5f);

        foreach (Collider collider in colliders)
        {
            // Check the tag of the collider to determine the terrain type
            if (collider.CompareTag("Grass"))
            {
                return "GrassFootstep"; // Return grass footstep sound
            }
            else if (collider.CompareTag("Water"))
            {
                return "WaterFootstep"; // Return water footstep sound
            }
        }

        return "DefaultFootstep"; // Default footstep sound if no specific terrain is detected
    }

    /*
    * 
    * Character Movement
    * 
    */

    private void HandleMovement()
    {

        // Get input for horizontal and vertical movement.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        // If player is not walking on water then crouch
        if(!isWalkingOnWater)
            HandleCrouch(); // Handling Crouch 
        
        HandleRunning(); //Handling Running
        
        // Check if the player has velocity (is moving) & If movemevent buttons are pressed
        if ((horizontal != 0 || vertical != 0) && characterController.velocity.magnitude > 0.1f)
        {
            // Calculate the terrain type based on the player's position.
            string terrainType = CalculateTerrainType();

            // Play the footstep sound based on the terrain type.
            footstepSound.PlayFootstepSound(terrainType);
        }
        else
        {
            // Stop footstep sound when there is no movement input.
            footstepSound.StopFootstepSound();
        }


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

        //Debug.Log("Move Speed: " + moveSpeed);

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
        // Get Input for running
        // * Removed !isCrouching so now the player can run 
        isRunning =  Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Check if the player is currently running.
        if (isRunning && characterController.velocity.magnitude > 0.1f)
        {
            // Increment the run timer.
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
        // If Stress is rising and player starts running
        else if ( isRunning && stressManager.currentStress > 0f ) // If Stress is rising and player starts running
        {
            stressManager.IncreaseStress(Time.deltaTime * runningStressIncreaseRate);
        }
        else
        {
            // If the player stops running and stress is at 0, reset the run timer.
            if (stressManager.currentStress == 0f)
            {
                runTimer = 0f;
            }
            
            // If player is not crouching then to return to default running speed
            if (!isCrouching)
                runningSpeed = originalRunningSpeed;
        }
        //If player is crouching then update to crouch running speed
        if (isCrouching)
            runningSpeed = crouchRunningSpeed;
        
    }

    /*
    * 
    * Crouching
    * 
    */
    
    // A property to manage the crouch permission
    public bool CanCrouch
    {
        get { return canCrouch; }
        set { canCrouch = value; }
    }

    private void HandleCrouch()
    {
        // Check if the player is in water, if so, automatically uncrouch.
    if (isWalkingOnWater && isCrouching)
    {
        // Restore original height and center.
        characterController.center = originalCenter;
        characterController.height = originalHeight;

        // Hide the crouch icon when standing.
        if (crouchIcon != null)
        {
            crouchIcon.enabled = false;
        }

        isCrouching = false;

        // Restore original movement speed when standing up.
        movementSpeed /= 0.5f;
    }
    else
    {
        // Check if the crouch input is pressed.
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // Check if the cube collides with anything above the player's head.
            if (isCrouching && HeadCubeCollides())
            {
                Debug.Log("Cannot uncrouch due to obstruction above.");
                return;
            }

            // Adjust character height and center based on crouch state.
            // Toggle the crouching state.
            if (!isCrouching)
            {
                // Reduce the character's height and adjust center when crouching.
                characterController.center = originalCenter * 0.5f;
                characterController.height = originalHeight * 0.5f;

                // Show the crouch icon when crouching
                if (crouchIcon != null)
                {
                    crouchIcon.enabled = true;
                }

                isCrouching = true;

                // Update Crouch movement speed to half of the original movement speed
                movementSpeed *= 0.5f;
            }
            else
            {
                // If the player is crouching, allow uncrouching.
                // Restore original height and center.
                //characterController.center = originalCenter;
                //characterController.height = originalHeight;
                StartCoroutine(UncrouchSmooth());

                // Hide the crouch icon when standing.
                if (crouchIcon != null)
                {
                    crouchIcon.enabled = false;
                }

                isCrouching = false;

                // Restore original movement speed when standing up.
                movementSpeed /= 0.5f;
            }
        }
    }
    }
    
    // Effectively uncrouching the character speed.
    private IEnumerator UncrouchSmooth()
    {
        float targetHeight = originalHeight; // Store the original standing height as the target height for uncrouching.
        float startHeight = characterController.height; // Store the current height of the character as the starting height for uncrouching.
        float distanceToTarget = Mathf.Abs(targetHeight - startHeight); // Calculate the absolute distance between the start height and the target height.
        float uncrouchDuration = distanceToTarget / uncrouchSpeed;  // Calculate the duration of the uncrouching speed based on the distance to travel and the uncrouch speed.

        float timer = 0f; // Initialize a timer to track the elapsed time during the uncrouching speed.

        while (timer < uncrouchDuration)
        {
            float t = timer / uncrouchDuration; // Calculate the normalized time value (t) within the range [0, 1] based on the current elapsed time and uncrouch duration.
            float smoothStep = Mathf.SmoothStep(0f, 1f, t); // Apply a smooth step function to the normalized time value to ensure a smooth transition in the character's height.
            characterController.height = Mathf.Lerp(startHeight, targetHeight, smoothStep); // gradually adjusting the character's height.

            timer += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;     // Ensure that the character's height is set to the target height accurately.
       
        // Update the isCrouching and isUncrouching flags to reflect the character's state.
        isCrouching = false;
        isUncrouching = false;
    }

    
    // Method to check if the cube collides with anything above the player's head preventing uncrouching.
    private bool HeadCubeCollides()
    {
        // Check if the headCube object reference is set
        if (headCube != null)
        {
            // Use Physics.BoxCast to check for collisions with the cube
            return Physics.BoxCast(headCube.transform.position, headCube.transform.localScale / 2, Vector3.up, Quaternion.identity, originalHeight);
        }
        else
        {
            // If the headCube reference is not set, return false
            return false;
        }
    }
    

    /*
    * 
    * Jumping
    * 
    */
    
    private void HandleJump()
    {
        // Check if the player is grounded.
        bool groundedPlayer = characterController.isGrounded;

        // Reset jump-related variables when grounded.
        if (groundedPlayer)
        {
            ResetJump(); // Reset jump-related variables when grounded.
            maxJumpHeight = transform.position.y; // Reset maxJumpHeight when grounded.
        }
        
        // Adjust maxJumpHeight when walking on water.
        maxJumpHeight = isWalkingOnWater ? transform.position.y / 2 : maxJumpHeight;

        ApplyGravity(); // Apply gravity force when in the air.
        Jump(); // Perform a jump or double jump if conditions are met.

        // Update the maxJumpHeight during the jump.
        maxJumpHeight = transform.position.y > maxJumpHeight ? transform.position.y : maxJumpHeight;
        
    }

    // Resets jump-related variables when the character is grounded.
    private void ResetJump()
    {
        verticalVelocity = 0f; // Reset vertical velocity when grounded.
        jumpsPerformed = 0; // Reset jump count when grounded.
        isJumping = false;
    }


    // Applies gravity force when the character is in the air.
    private void ApplyGravity()
    {
        // Only apply gravity when the character is falling.
        if (!characterController.isGrounded)
        {
            // Update the vertical velocity based on gravity.
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            // Reset vertical velocity when grounded.
            verticalVelocity = -gravity * Time.deltaTime;
        }
    }

    // Perform a jump or double jump if conditions are met..
    private void Jump()
    {
        // Check if the player is allowed to jump.
        bool canJump = !isCrouching;
        
        if (Input.GetButtonDown("Jump") && jumpsPerformed < maxJumps && canJump)
        {
            // Adjust jump height when walking on water.
            float adjustedJumpHeight = isWalkingOnWater ? jumpHeight / 2 : jumpHeight;

            verticalVelocity = Mathf.Sqrt(2 * adjustedJumpHeight  * gravity); // Calculate double jump velocity.
            isJumping = true; // Set jumping flag for double jump.
            jumpsPerformed++; // Increment jump count for double jump.
        }
    }


    // This checks if the player is currently falling and triggers stress
    // private void CheckFalls()
    // {
    //     // Check if the player is falling (not grounded and moving downward).
    //     if (!characterController.isGrounded && characterController.velocity.y < 0)
    //     {
    //         // Calculate the fall height based on the highest point reached during the jump.
    //         float fallHeight = maxJumpHeight - transform.position.y;
    //
    //         // Check if the fall height is greater than the specified threshold.
    //         if (fallHeight > fallHeightThreshold)
    //         {
    //             // Increment the falls count.
    //             fallsCount++;
    //
    //             // If the player falls twice their height, trigger stress.
    //             if (fallsCount >= 2)
    //             {
    //                 // Calculate stress increase as a percentage of the fall height.
    //                 float stressIncreasePercentage = Mathf.Clamp(fallHeight / fallHeightThreshold, 0f, maxFallinStressRange); 
    //                 float stressIncrement = 20f; 
    //
    //                 // Calculate the actual stress increase.
    //                 float stressIncrease = stressIncreasePercentage * stressIncrement;
    //
    //                 // Clamp the stress increase to a maximum value.
    //                 //stressIncrease = Mathf.Clamp(stressIncrease, 0f, 20f); // Adjust the upper limit as needed.
    //
    //                 // Increment stress by the calculated amount
    //                 stressManager.IncreaseStress(stressIncrease);
    //
    //                 // Reset falls count.
    //                 fallsCount = 0;
    //             }
    //         }
    //         else
    //         {
    //             // Reset falls count when the fall height is pointless.
    //             fallsCount = 0;
    //         }
    //     }
    //     else
    //     {
    //         // Reset falls count when grounded.
    //         fallsCount = 0;
    //
    //         //If the player not falling decrease stress
    //         stressManager.DecreaseStress(stressManager.stressDecreaseRate * Time.deltaTime);
    //
    //     }
    // }
    
    // Method to disable or enable movement
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
        // Check if the triggering object has the tag "Water".
        if (other.CompareTag("Water"))
        {
            isWalkingOnWater = true; // Set the flag to indicate that the character is walking on water.
            HandleCrouch();
        }
        
        // Check if the player object is the endpoint in the GameManager.
        if (other.gameObject == gameManager.endpoint)
        {
            isInEndpointTriggerZone = true; // Setting the flag to indicate that the character is in the endpoint trigger zone.
        }
        
        if (other.CompareTag("NoUnCrouchZone"))
        {
            CanCrouch = false;
        }
        
        // Check if the player entered a lilypad's trigger collider
        if (other.CompareTag("Lilypad"))
        {
            // Get the LilypadController component of the entered lilypad
            currentLilypad = other.GetComponent<LilyPadTrigger>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Check if the triggering object has the tag "Water".
        if (other.CompareTag("Water"))
        {
            isWalkingOnWater = false; // Set the flag to indicate that the character is not walking on water anymore.
            Debug.Log("Out of Water"); // Log a debug message indicating that the character is out of water.
        }
        
        // Check if the player object is the endpoint in the GameManager.
        if (other.gameObject == gameManager.endpoint)
        {
            isInEndpointTriggerZone = false; // Setting the flag to indicate that the character is not in the endpoint trigger zone anymore.
        }
        
        if (other.CompareTag("NoUnCrouchZone"))
        {
            CanCrouch = true;
        }
        
        // Check if the player exited a lilypad's trigger collider
        if (other.CompareTag("Lilypad"))
        {
            // Clear the current lilypad reference
            currentLilypad = null;
        }
    }
    
    // Property to get the current state of player being in the endpoint trigger zone.
    public bool IsInEndpointTriggerZone
    {
        get { return isInEndpointTriggerZone; }
    }
}
