using UnityEngine;

public class EnergySystem:MonoBehaviour,ISystem
    {
        public void Continue()
        {
            throw new System.NotImplementedException();
        }

        public void GameOver()
        {
            throw new System.NotImplementedException();
        }
        
        public void Restart()
        {
            Continue();
        }
    }