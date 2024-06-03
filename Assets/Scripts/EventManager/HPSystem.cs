using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HPSystem:MonoBehaviour,ISystem
{
    public GameObject Heart;
    public Transform ParentTranform;
    public List<GameObject> Hearts;
    
    private void Start()
    {
        Hearts = new List<GameObject>();
        EventManager.Instance.Systems.Add(this);
        for (int i = 0; i < EventManager.Instance.HPCur; i++)
        {
            Hearts.Add(Instantiate(Heart, ParentTranform));
        }
    }

    public void Continue()
    {
        EventManager.Instance.HPCur--;
        Hearts[0].SetActive(false);
        Hearts.RemoveAt(0);
    }
    
    public void Restart()
    {
        foreach (var VARIABLE in Hearts)
        {
            VARIABLE.SetActive(false);
        }
        EventManager.Instance.HPCur = EventManager.Instance.HP;
        Hearts = new List<GameObject>();
        for (int i = 0; i < EventManager.Instance.HPCur; i++)
        {
            Hearts.Add(Instantiate(Heart, ParentTranform));
        }
    }

    public void GameOver()
    {
    }
}