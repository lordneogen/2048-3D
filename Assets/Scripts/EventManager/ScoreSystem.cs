using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreSystem:MonoBehaviour,ISystem
{
    private TextMeshProUGUI TextMeshProUGUI;

        private void Start()
        {
            EventManager.Instance.Systems.Add(this);
            EventManager.Instance.ScoreEnc += ScoreAdd;
            //
            TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
            SetText(EventManager.Instance.Score);
            //
            Invoke("OnEnable",1f);
        }

        private void OnEnable()
        {
            try
            {
                SetText(EventManager.Instance.Score);
            }
            catch
            {
                return;
            }
        }

        public void Continue()
        {
            EventManager.Instance.HighScore = Mathf.Max(EventManager.Instance.Score, EventManager.Instance.HighScore);
            SetText(EventManager.Instance.Score);
        }

        private void ScoreAdd(int scoreAdd)
        {
            EventManager.Instance.Score += scoreAdd;
            EventManager.Instance.HighScore = Mathf.Max(EventManager.Instance.HighScore, EventManager.Instance.Score);
            SetText(EventManager.Instance.Score);
        }

        private void OnDisable()
        {
            EventManager.Instance.ScoreEnc -= ScoreAdd;
        }

        public void GameOver()
        {
            return;
        }
        private void SetText(int num)
        {
            // Debug.Log(ScoreSystem.Score.ToString());
            if(num==-1) TextMeshProUGUI.SetText("Score: 0");
            else TextMeshProUGUI.SetText("Score: "+EventManager.Instance.Score.ToString());
            transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f),0.3f);
            transform.DOScale(new Vector3(1f, 1f, 1f),0.3f);
        }
        
        public void Restart()
        {
            EventManager.Instance.HighScore = Mathf.Max(EventManager.Instance.Score, EventManager.Instance.HighScore);
            EventManager.Instance.Score=0;
            SetText(EventManager.Instance.Score);
        }

}