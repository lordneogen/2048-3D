using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SizeUpDown : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float Ampletude;
    [SerializeField] private float Seconds;
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(transform.localScale*Ampletude, Seconds));
        sequence.Append(transform.DOScale(transform.localScale, Seconds));
        sequence.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
