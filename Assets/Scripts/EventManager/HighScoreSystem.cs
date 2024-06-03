using System;
using TMPro;
using UnityEngine;

public class HighScoreSystem:MonoBehaviour,ISystem
{
    private TextMeshProUGUI TextMeshProUGUI;
    
    private void Start()
    {
        EventManager.Instance.Systems.Add(this);
        EventManager.Instance.ScoreEnc += ScoreAdd;
        //
        TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
        ScoreAdd(0);
    }

    private void ScoreAdd(int num)
    {
        if (!EventManager.Instance.new_record&&EventManager.Instance.Score>=EventManager.Instance.HighScore)
        {
            EventManager.Instance.new_record = true;
            //
            TextMeshProUGUI.SetText("New record"); 
            EventManager.Instance.EffectSystem.Play(EventManager.Instance.EffectSystem.HighScoreSystem);
        }
        else
        {
            if(EventManager.Instance.Score<EventManager.Instance.HighScore)
            TextMeshProUGUI.SetText((EventManager.Instance.HighScore - EventManager.Instance.Score).ToString() +
                                    " score record left");
        }
    }

    public void Continue()
    {
        TextMeshProUGUI.SetText((EventManager.Instance.HighScore ).ToString() +
                                " score record left");
        EventManager.Instance.new_record = false;
    }

    public void GameOver()
    {
        TextMeshProUGUI.SetText((EventManager.Instance.HighScore ).ToString() +
                                " score record left");
    }
    
    public void Restart()
    {
        Continue();
    }
}