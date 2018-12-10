using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateLock : MonoBehaviour {

    

    //True enable Vsync (Framerate = Refresh Rate), False disables it.
    public bool VSync;

    //If VSync is disabled, you can set a specific target framerate
    public int targetFrameRate;

    // Use this for initialization
    void Start()
    {
        if (VSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (!VSync && (targetFrameRate != Application.targetFrameRate))
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
