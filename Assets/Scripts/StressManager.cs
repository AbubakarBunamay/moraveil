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
    public CanvasGroup stressCanvasGroup; // Reference to the CanvasGroup component.

    private float currentStress = 0f; // Current stress level.

    private bool playerInsideTrigger = false; // By default player not inside trigger

    [SerializeField]
    private CameraShakeManager shakeManager; // Reference to the CameraShakeManager component.

    [SerializeField]
    private float maxShakeMagnitude; // Maximum camera shake magnitude.

    [SerializeField]
    private float maxShakeDuration; // Maximum camera shake duration.

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone.
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone.
        if (other.CompareTag(playerTag))
        {
            playerInsideTrigger = false;
        }
    }
    private void Start()
    {
        // Check if the CameraShakeManager reference is not set, then try to find it.
        if (shakeManager == null)
        {
            shakeManager = GameObject.FindAnyObjectByType<CameraShakeManager>();
        }
    }

    private void Update()
    {
        // Update stress, trigger stress effects, and update UI.
        TriggerStress();
        IncreaseStress();
        // Update the stress meter UI.
        //stressMeterUI.fillAmount = currentStress / maxStress;

        
    }

    private void TriggerStress()
    {
            // Calculate camera shake parameters based on stress.
            float normalizedStress = currentStress / maxStress;
            float shakeMagnitude = normalizedStress * maxShakeMagnitude;
            float shakeDuration = normalizedStress * maxShakeDuration;

            // Toggle the darkening effect and camera shake based on stress level.
            if (currentStress > 0)
            {
                // Fade in the image based on stress level.
                stressCanvasGroup.alpha = normalizedStress;

                // Trigger camera shake
                shakeManager.ShakeCamera(shakeDuration, shakeMagnitude);

            }
            else
            {
                // Fade out the image when stress is zero.
                stressCanvasGroup.alpha = 0f;
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
