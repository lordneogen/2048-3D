using DG.Tweening;
using UnityEngine;

public class Fall:MonoBehaviour
{
    public float Speed;
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Speed, 0), 1f));
        // sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Ampletude, 0), Seconds));
        sequence.SetLoops(-1);
    }
}