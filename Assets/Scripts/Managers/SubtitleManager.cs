using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;
    private Coroutine subtitleCoroutine;

    public void ShowSubtitle(string text, float duration)
    {
        if (subtitleCoroutine != null)
        {
            StopCoroutine(subtitleCoroutine);
            subtitleText.text = "";
        }
        // Activate the subtitle object
        subtitleText.gameObject.SetActive(true);

        subtitleText.text = text;
        subtitleCoroutine = StartCoroutine(HideSubtitle(duration));
    }

    private IEnumerator HideSubtitle(float delay)
    {
        yield return new WaitForSeconds(delay);
        subtitleText.text = "";

        // Activate the subtitle object
        subtitleText.gameObject.SetActive(false);
    }
}
