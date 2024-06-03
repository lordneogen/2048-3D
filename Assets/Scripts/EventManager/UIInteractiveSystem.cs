using System;
using UnityEngine;
using UnityEngine.UI;


public class UIInteractiveSystem:MonoBehaviour
{
    public Slider BackVol;
    public Slider MainVol;
    public Slider Sens;
    public Toggle Double;

    private void OnEnable()
    {
        try
        {
            BackVol.value = EventManager.Instance.audioManager.Backvol;
            MainVol.value = EventManager.Instance.audioManager.Mainvol;
            Sens.value = EventManager.Instance.RotateObjectWithMouse.rotationSpeedMobile;
            Double.isOn = EventManager.Instance.doubleClick;
        }
        catch
        {
            
        }
    }
}