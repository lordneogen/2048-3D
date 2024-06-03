

using System;
using TMPro;
using UnityEngine;

public class FreezeSpeedTextSystem:MonoBehaviour,ISystem
{
    private TextMeshProUGUI TextMeshProUGUI;
    private void Start()
    {
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        EventManager.Instance.Systems.Add(this);
        TextMeshProUGUI.SetText(EventManager.Instance.StopRateBase.ToString());
    }

    public void Continue()
    {
        TextMeshProUGUI.SetText(EventManager.Instance.StopRateBase.ToString());

    }
    
    public void Restart()
    {
        Continue();
    }

    public void GameOver()
    {
        TextMeshProUGUI.SetText(EventManager.Instance.StopRateBase.ToString());

    }
}