using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 

public class StressManager : MonoBehaviour
{
    public float maxStress = 100f; // Maximum stress level.
    public float stressIncreaseRate = 10f; // Rate at which stress increases per second.
    public float stressDecreaseRate = 5f; // Rate at which stress decreases per second.
    public string playerTag = "Player"; // Tag of the player GameObject.
    public Image stressMeterBar; // Reference to the stress meter UI element.
    public Image darkScreen; // Reference to the full-screen darkening effect.
    public CanvasGroup stressCanvasGroup; // Reference to the CanvasGroup component.

    public float currentStress = 0f; // Current stress level.

    public bool playerInsideTrigger = false; // By default player not inside trigger

    [SerializeField]
    private CameraShakeManager shakeManager; // Reference to the CameraShakeManager component.

    [SerializeField]
    private float maxShakeMagnitude; // Maximum camera shake magnitude.

    [SerializeField]
    private float maxShakeDuration; // Maximum camera shake duration.

    public Volume volume; // Reference to the Post-Processing Volume component

    public bool isPlayerDead = false; // Variable to track player's life status.

    private float stunCooldown = 0f; // Cooldown for the stun effect.

    public float stunDuration = 5f; // Duration of the stun effect in seconds.

    private bool isStunned = false; // Flag to indicate whether the player is currently stunned.

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
        // Check if the Volume component is found
        if(volume == null)
        volume = FindObjectOfType<Volume>();

        // Check if the CameraShakeManager reference is not set, then try to find it.
        if (shakeManager == null)
        {
            shakeManager = GameObject.FindAnyObjectByType<CameraShakeManager>();
        }
    }

    private void Update()
    {
        // Update stress, trigger stress effects, and update UI.
        //StressEffects();
        //IncreaseStressTrigger();

        if (currentStress >= maxStress && !isPlayerDead)
        {
            // Check if the stun cooldown has passed.
            if (stunCooldown <= 0f)
            {
                // Trigger stun effect.
                StunPlayer();
                stunCooldown = stunDuration; // Set the cooldown for the stun effect.
            }
        }

        // Update stun cooldown timer.
        if (stunCooldown > 0f)
        {
            stunCooldown -= Time.deltaTime;
        }

        if (currentStress >= maxStress)
        {
            // Reset stress after triggering stun.
            currentStress = 0f;
        }

        if (isStunned)
        {
            // Keep stress at max while stunned.
            currentStress = maxStress;
        }
        else
        {
            // Gradually decrease stress when not stunned.
            currentStress = Mathf.Clamp(currentStress - stressDecreaseRate * Time.deltaTime, 0f, maxStress);
        }

    }

    public void StressEffects()
    {
        // Calculate camera shake parameters based on stress.
        float normalizedStress = currentStress / maxStress;
        float shakeMagnitude = normalizedStress * maxShakeMagnitude;
        float shakeDuration = normalizedStress * maxShakeDuration;

        //float desiredOpacity = Mathf.Clamp(normalizedStress, 0.0f, 0.5f);

        // Check if the Vignette effect is available
        if (volume != null)
        {
            // Try to get the Vignette effect from the volume's profile
            if (volume.profile.TryGet(out Vignette vignette))
            {
                // Adjust the vignette intensity based on stress level
                vignette.intensity.value = normalizedStress;
            }

            // Try to get the Film Grain effect from the volume's profile
            if (volume.profile.TryGet(out FilmGrain fg))
            {
                // Adjust the Film Grain intensity based on stress level
                fg.intensity.value = normalizedStress;
            }

            // Trigger camera shake
            shakeManager.ShakeCamera(shakeDuration, shakeMagnitude);
        }
        

        /*// Toggle the darkening effect and camera shake based on stress level.
        if (currentStress > 0)
        {
            // Fade in the image based on stress level.
            // stressCanvasGroup.alpha = desiredOpacity;
            stressCanvasGroup.alpha = normalizedStress;

            // Trigger camera shake
            shakeManager.ShakeCamera(shakeDuration, shakeMagnitude);

        }
        else
        {
            // Fade out the image when stress is zero.
            stressCanvasGroup.alpha = 0f;
        }*/
    }

    public void IncreaseStress(float stressAmount)
    {

        currentStress += stressAmount;
        // Trigger stress effects.
        if(currentStress > 0)
        {
            StressEffects();

        }
        else 
        {
            ResetStressEffects();
        }
        currentStress = Mathf.Clamp(currentStress, 0f, maxStress);

    }

    public void DecreaseStress(float stressAmount)
    {
        currentStress -= stressDecreaseRate * Time.deltaTime;
        currentStress = Mathf.Clamp(currentStress, 0f, maxStress);
        Debug.Log("Decreasing stress: " + currentStress);
    }

    public void IncreaseStressTrigger()
    {

        currentStress += stressIncreaseRate * Time.deltaTime;
        currentStress = Mathf.Clamp(currentStress, 0f, maxStress);
        Debug.Log("Increasing stress: " + currentStress);

    }

    public void DecreaseStressTrigger()
    {
        if (currentStress > 0)
        {
            currentStress -= stressDecreaseRate * Time.deltaTime;
            currentStress = Mathf.Clamp(currentStress, 0f, maxStress);
            Debug.Log("Decreasing stress: " + currentStress);
        }
    }

    public void ResetStressEffects()
    {
        // Reset Vignette intensity
        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.value = currentStress / maxStress;
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value, 0f, maxStress);
        }

        // Reset Film Grain intensity
        if (volume.profile.TryGet(out FilmGrain fg))
        {
            fg.intensity.value = currentStress / maxStress;
            fg.intensity.value = Mathf.Clamp(fg.intensity.value, 0f, maxStress);

        }
    }

    public void IncreaseSafezoneStress()
    {
        // Player is inside the trigger zone.
        if (currentStress < maxStress * 0.3f)
        {
            // Limit stress increase to 30%.
            float stressIncrease = stressIncreaseRate * Time.deltaTime;
            float remainingStressSpace = maxStress * 0.3f - currentStress;

            if (remainingStressSpace > 0)
            {
                // Increase stress only if it won't exceed 30%.
                currentStress += Mathf.Min(stressIncrease, remainingStressSpace);
                Debug.Log("Increasing stress: " + currentStress);
            }
        }
    }

    public void StunPlayer()
    {
        StartCoroutine(StunCoroutine());
        // Set the cooldown for the stun effect.
        stunCooldown = stunDuration;
    }

    private IEnumerator StunCoroutine()
    {
        // Set the flag indicating that the player is stunned.
        isStunned = true;

        Debug.Log("Player Stunned!");

        // Disabling Character Movement
        FPSController playerMovement = FindObjectOfType<FPSController>();
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        // Wait for the stun duration.
        yield return new WaitForSeconds(stunDuration);

        // Re-enable player movement after the stun duration.
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }

        Debug.Log("Player Unstunned!");

        // Reset the flag after the stun effect is over.
        isStunned = false;
    }


}