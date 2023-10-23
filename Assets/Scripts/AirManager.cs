using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirManager : MonoBehaviour
{
    public float maxAir = 100f;
    public float airDepletionRate = 1f;
    
    public StressManager stressManager;

    private float currentAir;
    
    private bool isUnderwater = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAir = maxAir;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(isUnderwater) {
            currentAir -= airDepletionRate * Time.deltaTime;
            
            if (currentAir < 0)
            {
                currentAir = 0;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isUnderwater = false;
        }
    }
}
