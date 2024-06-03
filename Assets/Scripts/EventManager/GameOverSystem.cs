using System;
using System.Linq;
using UnityEngine;

public class GameOverSystem:MonoBehaviour,ISystem
    {
        //
        public GameObject GameOverUI;
        //
        private void Start()
        {
            GameOverUI.SetActive(false);
            EventManager.Instance.Systems.Add(this);
        }

        public void Continue()
        {
            Time.timeScale = 1f;
            GameOverUI.SetActive(false);
        }
        //
        public void GameOver()
        {
            Time.timeScale = 0f;
            GameOverUI.SetActive(true);
        }
        
        public void Restart()
        {
            Continue();
        }
    }