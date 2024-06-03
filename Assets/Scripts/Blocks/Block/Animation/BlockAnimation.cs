// namespace Blocks.Block.Animation
// {
//     public class BlockAnimation
//     {
//         
//     }
// }

using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

[System.Serializable]
public class BlockAnimation
{
    [SerializeField]
    private Color _endcolor;
    public Color _start;
    [HideInInspector] public Sequence _Shakesequence=DOTween.Sequence();
    [SerializeField]
    public float delay;
    public void SizeUP(Transform transform, float startVal, float endVal, int loop = 1, Action fun = null)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one * startVal, delay/4));
        sequence.Append(transform.DOScale(Vector3.one * endVal, delay*2));
        sequence.OnComplete(() =>
        {
            // Check if fun is not null before invoking
            fun?.Invoke();
        });
        sequence.SetLoops(1);
    }

    public void MoveTo(Transform transform, Vector3 endVal, int loop = 1,Action fun=null)
    {
        transform.DOLocalMove(endVal , delay).OnComplete(()=>{fun.Invoke();});
    }
    
    public void Shake(Transform transform,float strength)
    {
        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
        _Shakesequence.Kill();
        _Shakesequence = DOTween.Sequence();
        _Shakesequence.Append((meshRenderer.material.DOColor(_endcolor*strength+_start*(Mathf.Abs(1-strength)), delay)));
        // _Shakesequence.Append((meshRenderer.material.DOColor(_start, delay)));
        _Shakesequence.SetLoops(1);
    }
}