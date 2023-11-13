using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager instance;

    public static ReferenceManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ReferenceManager();
                if(instance != null)
                {
                    GameObject obj = new GameObject("ReferenceManager");
                    instance = obj.AddComponent<ReferenceManager>();
                }
            }
            return instance;
        }
    }

    //References to go here
    public FlashLightcontroller flashLightcontroller;
    public FPSController FPSController;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance=this;
            DontDestroyOnLoad(this.gameObject);
        }

        flashLightcontroller = GetComponent<FlashLightcontroller>();
    }
    
    public void ClearAllReference()
    {
        //To reset maybe
    }
}
