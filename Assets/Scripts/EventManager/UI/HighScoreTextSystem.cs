using System;
using TMPro;
using UnityEngine;

public class HighScoreTextSystem:MonoBehaviour,ISystem
{
    private TextMeshProUGUI TextMeshProUGUI;
    
    private void Start()
    {
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        EventManager.Instance.Systems.Add(this);
        TextMeshProUGUI.SetText("High score:"+EventManager.Instance.HighScore);
    }

    private void OnEnable()
    {
        try
        {
            TextMeshProUGUI.SetText("High score:" + EventManager.Instance.HighScore);
        }
        catch
        {
            
        }
    }

    public void Continue()
    { 
        TextMeshProUGUI.SetText("High score:"+EventManager.Instance.HighScore);

    }

    public void Restart()
    {
        Continue();
    }

    public void GameOver()
    {
        TextMeshProUGUI.SetText("High score:"+EventManager.Instance.HighScore);
    }
}