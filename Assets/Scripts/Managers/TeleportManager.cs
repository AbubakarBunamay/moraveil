using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    
    [SerializeField] Transform[] roomPositions; // Array to store positions of the rooms
    [SerializeField] GameObject fpsController; // Reference to FPSController
    [SerializeField] private TipsPopup tipsPopup; // Reference to the TipsPopup component
    [SerializeField] private string MapTipPopup= "Grab the Map"; // The Map tipPopup Text
    [SerializeField] GameObject[] doors; // Array to store references to all doors
    private bool allDoorsOpened = false; // Flag to track if all doors have been opened

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
           
           // Check if the player has teleported to the map room (7)
           if (roomNumber == 7 && !allDoorsOpened)
           {
               // Open all doors
               OpenDoor(doors);
               allDoorsOpened = true; // Set flag to true to indicate all doors are opened
               tipsPopup.DisplayTipMessage(MapTipPopup,5);
           }
            // Re-enable FPSController script after teleportation
            fpsController.SetActive(true);
        }
    }
    
    void OpenDoor(GameObject[] doorsToOpen)
    {
        foreach (GameObject doorToOpen in doorsToOpen)
        {
            float elapsedTime = 0f;
            Vector3 initialPosition = doorToOpen.transform.position;
            Vector3 targetPosition = initialPosition + Vector3.up * 10f;

            while (elapsedTime < 1f)
            {
                doorToOpen.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);
                elapsedTime += Time.deltaTime * 2f;
            }

            // Ensure the door reaches the final position
            doorToOpen.transform.position = targetPosition;
        }
    }

}
