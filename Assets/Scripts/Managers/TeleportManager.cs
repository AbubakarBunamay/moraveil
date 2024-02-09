using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    // Array to store positions of the rooms
    public Transform[] roomPositions;
    
    // Reference to FPSController
    public GameObject fpsController;

    // Update is called once per frame
    void Update()
    {
        // Check for the Alt key, Shift key, and room number inputs
        if ((Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift)))
        {
            for (int i = 1; i <= roomPositions.Length; i++)
            {
                // Check for the corresponding number key
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                {
                    //Debug.Log("Teleport key pressed: Alt + " + i);
                    // Teleport the player to the corresponding room
                    TeleportPlayer(i);
                    break;
                }
            }
        }
    }

    void TeleportPlayer(int roomNumber)
    {
        // Ensuring the room number is within valid range
        if (roomNumber >= 1 && roomNumber <= roomPositions.Length)
        {
            // Temporarily disabling FPSController script
            fpsController.SetActive(false);

            // Teleport the player to the corresponding room position
            fpsController.transform.position = roomPositions[roomNumber - 1].position;
           //Debug.Log("Player teleported to room " + roomNumber);

            // Re-enable FPSController script after teleportation
            fpsController.SetActive(true);
        }
    }
}
