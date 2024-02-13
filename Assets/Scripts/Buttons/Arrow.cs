// using System.Numerics;

using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 Move;
    public bool x;
    public bool y;
    public bool z;
    public bool flip;
    private EventManager eventManager;
    [HideInInspector] public static Action<int, int, int> MoveBlock;
    
    private void Start()
    {
        eventManager = EventManager.Instance;
        if (x) Move = transform.right;
        else if (y) Move = transform.up;
        else if (z) Move = transform.forward;
        if (flip) Move = -Move;
    }
    private void OnMouseDown()
    {
        MoveBlock?.Invoke((int)Move.x,(int)Move.y,(int)Move.z);
        Debug.Log(Move);
    }
}