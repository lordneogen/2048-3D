using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class EventManager:MonoBehaviour
{

    public int Score=0;
    public GameObject BlockPref;
    public static EventManager Instance;
    public int Size;
    public float SizeBox;
    public Vector3 InitialVector;
    [HideInInspector] public BlocksSystem BlocksSystem;
    
    private void Awake()
    {
        Size += 2;
        Instance = this;
        Instance.Size=Size;
        Instance.SizeBox = SizeBox;
        Instance.InitialVector = InitialVector;
        Instance.BlockPref = BlockPref;
    }

    public void ScoreInc(int score)
    {
        Score += score;
    }
}