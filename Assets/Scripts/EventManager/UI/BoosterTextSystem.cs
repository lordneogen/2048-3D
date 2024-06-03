using System;
using TMPro;
using UnityEngine;
public class BoosterTextSystem:MonoBehaviour,ISystem
{
    private TextMeshProUGUI TextMeshProUGUI;

    private void Start()
    {
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        EventManager.Instance.Systems.Add(this);
        TextMeshProUGUI.SetText("Bonuses:"+EventManager.Instance.BonusesCount);
    }

    private void OnEnable()
    {
        try
        {
            TextMeshProUGUI.SetText("Bonuses:" + EventManager.Instance.BonusesCount);
        }
        catch
        {
            
        }
    }
    
    public void Restart()
    {
        Continue();
    }

    public void Continue()
    { 
        TextMeshProUGUI.SetText("Bonuses:"+EventManager.Instance.BonusesCount);
    }

    public void GameOver()
    {
        TextMeshProUGUI.SetText("Bonuses:"+EventManager.Instance.BonusesCount);
    }
}