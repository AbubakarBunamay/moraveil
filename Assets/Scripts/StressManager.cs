using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    public float maxStress = 100f; // Maximum stress level.
    public float stressIncreaseRate = 10f; // Rate at which stress increases per second.
    public float stressDecreaseRate = 5f; // Rate at which stress decreases per second.
    public string playerTag = "Player"; // Tag of the player GameObject.
    //public Image stressMeterUI; // Reference to the stress meter UI element.
    public Image darkScreen; // Reference to the full-screen darkening effect.

    private float currentStress = 0f;

    private bool playerInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = false;
        }
    }

    private void Update()
    {
        TriggerStress();
        IncreaseStress();
        // Update the stress meter UI.
        //stressMeterUI.fillAmount = currentStress / maxStress;

        
    }

    private void TriggerStress()
    {
        // Toggle the darkening effect based on stress level.
        if (currentStress > 0)
        {
            darkScreen.color = new Color(0f, 0f, 0f, currentStress / maxStress);
        }
        else
        {
            darkScreen.color = Color.clear;
        }
    }

    private void IncreaseStress()
    {

        // Increase stress if the player is inside the trigger zone.
        if (playerInsideTrigger)
        {
            currentStress += stressIncreaseRate * Time.deltaTime;
        }
        else
        {
            // Decrease stress if the player is outside the trigger zone.
            currentStress -= stressDecreaseRate * Time.deltaTime;
        }

        // Ensure stress stays within the 0 to maxStress range.
        currentStress = Mathf.Clamp(currentStress, 0f, maxStress);

    }


}
