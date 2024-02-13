using System;
using DG.Tweening;
using UnityEngine;

public class BlockModule:MonoBehaviour
{
    private Block Block;
    public int Num;

    private void Start()
    {
        Block = GetComponent<Block>();
    }

    public void NumIcr()
    {
        Num += Num;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one*0.29f,0.3f));
        sequence.Append(transform.DOScale(Vector3.one*0.24f,0.3f));
        sequence.SetLoops(1);
        Block.BlockView.Increase(Num);
    }

    public void MoveTo(int x,int y,int z,float Delay)
    {
        transform.DOLocalMove(new Vector3(x, y, z) * Delay, 0.15f);
        // transform.localPosition = new Vector3(x, y, z) * Delay;
    }
}