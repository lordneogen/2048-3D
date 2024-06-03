#if UNITY_EDITOR
using System;
using UnityEngine;

public class EditorOnlyBehaviour : MonoBehaviour
{
    private int screen_num = 1;
    public bool ScreenShot;
    private void Start()
    {
        Debug.Log("UNITY EDITOR");
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (ScreenShot){ ScreenCapture.CaptureScreenshot(screen_num.ToString()+"screen.png");
                screen_num++;
            }
            
        }
    }
}
#endif