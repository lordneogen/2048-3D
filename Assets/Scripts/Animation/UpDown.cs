using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float Ampletude;
    [SerializeField] private float Seconds;
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, -Ampletude, 0), Seconds));
        sequence.Append(transform.DOBlendableMoveBy(new Vector3(0, Ampletude, 0), Seconds));
        sequence.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
