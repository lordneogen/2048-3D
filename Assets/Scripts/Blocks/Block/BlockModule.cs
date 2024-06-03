using System;
using System.Collections;
using System.Drawing;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class BlockModule:MonoBehaviour
{
    
    [Header("Блок")]
    [SerializeField]
    private Block _block;
    [Header("Число")]
    private int _initialNum;
    public int Num;
    [Header("Замарозка")]
    public int RateStop;
    [HideInInspector]
    public int MoveIndex;
    [HideInInspector]
    public bool Stop;
    private void Awake()
    {
        _initialNum = Num;
    }
    
    private void OnEnable()
    {
        Num = _initialNum;
        NumIncRandom();
        NumIncRandom();
        _block.BlockView.IncreaseView(Num);
    }

    private void NumIncRandom()
    {
        if (Random.Range(0, 11) > 5) Num += Num;
        _block.BlockView.IncreaseView(Num);
    }

    public void NumIcr()
    {
        //
        MoveIndex = 0;
        Num += Num;
        //
        _block.BlockView._BlockAnimation.SizeUP(transform,0.24f,0.32f);
        _block.BlockView._BlockAnimation.SizeUP(transform,0.32f,0.24f);
        //
        _block.BlockView.IncreaseView(Num);
        EventManager.Instance.ScoreEnc.Invoke(Num);
    }

    public void MoveTo(int x,int y,int z,float Delay,bool Des=false)
    {
        MoveIndex = 0;
        _block.BlockView._BlockAnimation.MoveTo(transform,new Vector3(x,y,z)*Delay,fun: () =>
        {
            if (Des)
            {
                gameObject.SetActive(false);
                _block.BlockView._BlockAnimation.SizeUP(transform, 0.24f, 0.1f);
                _block.BlockView._BlockAnimation.SizeUP(transform, 0.1f, 0.24f);
            }
        });
        _block.BlockView._BlockAnimation.Shake(transform,(float)MoveIndex/(float)RateStop);

    }

    public void MoveDec()
    { 
        if (_block == null) return; // Check if the BlockModule object is null
        if(Stop) return;
        MoveIndex++;
        _block.BlockView._BlockAnimation.Shake(transform,(float)MoveIndex/(float)RateStop);
        if (MoveIndex >= RateStop) {Stop = true; _block.BlockView.DisableView(); }
    }

}