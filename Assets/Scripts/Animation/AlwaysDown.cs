using DG.Tweening;
using UnityEngine;
public class AlwaysDown:MonoBehaviour
    {
        [SerializeField] private float Ampletude;
        [SerializeField] private float Seconds;
        void Start()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Ampletude, 0), Seconds));
            // sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Ampletude, 0), Seconds));
            sequence.SetLoops(-1);
        }   
    }