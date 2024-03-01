using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Subtitle Text")]
public class SubtitleTexts : ScriptableObject
{
        [SerializeField] [TextArea] public string[] dialogue; // Array to hold dialogue lines for the subtitle sequence
        [SerializeField] public float[] pauseUntilNextLine; // Array to hold pause durations between each dialogue line

}
