using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class FadeMusic : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    private bool isGoingDown = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mixer.GetFloat("Volume", out float value);
            if(isGoingDown)
            {
                value -= 5f;
                mixer.SetFloat("Volume", value);
                if (value <= -80f)
                {
                    isGoingDown = false;
                }
            }
            else
            {
                value += 5f;
                mixer.SetFloat("Volume", value);
                if (value >= 0f)
                {
                    isGoingDown = true;
                }
            }
        }
    }
}
