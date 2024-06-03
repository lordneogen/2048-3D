using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoosterSystem:MonoBehaviour,ISystem
{
    public List<GameObject> Boosters;
    public float boosterTime;
    public float startTime;
    public bool is_over=false;
    [SerializeField] private float Range;
    private void Start()
    {
        EventManager.Instance.Systems.Add(this);
        InvokeRepeating("CreateBooster",startTime,boosterTime);
    }

    public void CreateBooster()
    {
        Vector3 pos = new Vector3(0, Random.Range(-Range,Range), 0);
        if (!is_over)
        {
            GameObject booster =Instantiate(Boosters[Random.Range(0, Boosters.Count)], transform);
            booster.transform.position = pos;
        }
    }

    public void Continue()
    {
        is_over = false;
    }

        public void GameOver()
        {
            is_over = true;
        }
    
        public void Restart()
        {
            Continue();
        }
    }
    